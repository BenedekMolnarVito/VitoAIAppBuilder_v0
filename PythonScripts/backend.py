# backend.py

import openai
import os
from typing import List
from docx import Document
import zipfile


import openpyxl
import PyPDF2
from textwrap import dedent
from pydantic import BaseModel
import json

client = openai.OpenAI(api_key=os.getenv("OPENAI_API_KEY"))
MODEL = "gpt-4o-mini"


class GeneratedCode(BaseModel):
    class GeneratedFile(BaseModel):
        filedirectory: str
        filename: str
        code: str

    files: List[GeneratedFile]

# TODO 7: Connect to SQL implementation?   

def load_word_document(file_path: str) -> str:
    doc = Document(file_path)
    full_text = []
    for para in doc.paragraphs:
        full_text.append(para.text)
    return ['\n'.join(full_text)]

def load_excel_document(file_path: str) -> str:
    wb = openpyxl.load_workbook(file_path)
    sheet = wb.active
    data = []
    for row in sheet.iter_rows(values_only=True):
        data.append('\t'.join([str(cell) if cell is not None else '' for cell in row]))
    return ['\n'.join(data)]

def load_pdf_document(file_path: str) -> str:
    with open(file_path, 'rb') as f:
        reader = PyPDF2.PdfReader(f)
        text = []
        for page in reader.pages:
            page_text = page.extract_text()
            if page_text:
                text.append(page_text)
    return ['\n'.join(text)]

def load_zip_document(file_path: str) -> List[str]:
    file_contents = []
    with zipfile.ZipFile(file_path, 'r') as zip_ref:
        for file_info in zip_ref.infolist():
            if not file_info.is_dir():
                file_name = file_info.filename
                if file_name.endswith(('.cs', '.txt', '.resx', '.java', '.php', '.html', '.js', '.css', '.scss', '.md', '.json', '.yaml', '.toml', '.xml', '.yaml')):
                    with zip_ref.open(file_name) as file:
                        content = file.read().decode('utf-8')
                        file_contents.append(content)
    return file_contents

def load_document(file_path: str) -> str:
    _, ext = os.path.splitext(file_path)
    ext = ext.lower()
    if ext == '.docx':
        return load_word_document(file_path)
    elif ext in ['.xlsx', '.xlsm', '.xltx', '.xltm']:
        return load_excel_document(file_path)
    elif ext == '.pdf':
        return load_pdf_document(file_path)
    elif ext == '.zip':
        return load_zip_document(file_path)
    else:
        raise ValueError(f"Unsupported file type: {ext}")

# TODO 5: Implement proper RAG model instead of embedding!! --> LlamaIndex? 
    #         # TODO 6: Implement function calls?   
def embed_text(text: str, model="text-embedding-3-small") -> list:
    embedding = client.embeddings.create(input=[text], model=model)
    return embedding.data[0].embedding


def load_json_file(file_path: str) -> dict:
    with open(file_path, 'r', encoding='utf-8') as file:
        data = json.load(file)
    return data

def generate_code(prompt: str, context: str) -> str:
    messages_data = load_json_file('messages_data.json')
    combined_prompt = f"Context:\n{context}\n\nPrompt:\n{prompt}\n\nGenerate the corresponding code:"
    system_content = """You are a helpful assistant that generates code based on provided context and prompts.
                        Your answer can be multiple code files, but each needs to have a file name (filename) and a path to the directory (filedirectory) it should be copied to.
                        Keep the code clean and concise. Do not repeat code, and adhere to SOLID principles.
                        If you need to add explanation, add it as comments in the code.
                        Do not include ['''csharp] at the start of code answers.
    """

    messages = [
        {"role": "system", "content": dedent(system_content)}
    ]

    for sequence in messages_data['message_sequences']:
        messages.extend(sequence['messages'])
        
    messages.append({"role": "user", "content": combined_prompt})

    res_form = {
        "type": "json_schema",
        "json_schema": {
            "name": "GeneratedCode",
            "schema": {
                "type": "object",
                "properties": {
                    "GeneratedFiles": {
                        "type": "array",
                        "items": {
                            "type": "object",
                            "properties": {
                                "FileDirectory": {"type": "string"},
                                "FileName": {"type": "string"},
                                "Code": {"type": "string"},
                            },
                            "required": ["FileDirectory", "FileName", "Code"],
                            "additionalProperties": False
                        }
                    }
                },
                "required": ["GeneratedFiles"],
                "additionalProperties": False
            },
            "strict": True
        }
    }

    response = client.chat.completions.create(
        model=MODEL,
        messages=messages,
        response_format = res_form,
        temperature=0.1,
    )

    return response.choices[0].message.content
