import os
import numpy as np
import psycopg2
from openai import OpenAI
from llama_index.core import SimpleDirectoryReader
from sklearn.metrics.pairwise import cosine_similarity

client = OpenAI(api_key=os.getenv("OPENAI_API_KEY"))

conn = psycopg2.connect(
    dbname="RAGTest",
    user="vito-ai",
    password="vito1234",
    host="localhost",
    port="5432"
)

cur = conn.cursor()


def init_db():
    cur.execute("""
    CREATE TABLE IF NOT EXISTS embeddings (
        id SERIAL PRIMARY KEY,
        doc_id varchar(100),
        file_name varchar(255),      
        file_path varchar(255)      
    );

    CREATE TABLE IF NOT EXISTS chunks (
        id SERIAL PRIMARY KEY,
        document_id INT NOT NULL,
        chunk_text TEXT,
        embedding VECTOR(1536)
    );

    ALTER TABLE chunks
    ADD CONSTRAINT IF NOT EXISTS fk_document_id
    FOREIGN KEY (document_id)
    REFERENCES embeddings (id)
    ON DELETE CASCADE;
    """)
    conn.commit()

def store_document(doc_id: str, file_name: str, file_path: str):
    cur.execute(
        "SELECT id FROM embeddings WHERE file_name = %s",
        (file_name,)
    )
    existing_id = cur.fetchone()

    if existing_id:
        cur.execute(
            "DELETE FROM embeddings WHERE id = %s",
            (existing_id[0],)
        )
        # conn.commit()

    cur.execute(
        "INSERT INTO embeddings (doc_id, file_name, file_path) VALUES (%s, %s, %s)",
        (doc_id, file_name, file_path)
    )
    # conn.commit()
    result_id = cur.execute(
    "SELECT id FROM embeddings WHERE doc_id = %s",
    (doc_id,)
    )

    result_id = cur.fetchall()
    return result_id[0][0]

def store_chunk(document_id: int, chunk_text: str, embedding: np.ndarray):
    cur.execute(
        "INSERT INTO chunks (document_id, chunk_text, embedding) VALUES (%s, %s, %s)",
        (document_id, chunk_text, embedding.tolist())
    )
    conn.commit()

def process_and_store_docs(documents, chunk_size: int = 1000):
    for doc in documents:
        document_id = store_document(doc.doc_id, doc.metadata["file_name"], doc.metadata["file_path"])
        text_chunks = [doc.text[i:i + chunk_size] for i in range(0, len(doc.text), chunk_size)]
        for chunk_text in text_chunks:
            embedding = embed_text(chunk_text)
            store_chunk(document_id, chunk_text, embedding)

def load_documents(directory_path: str):
    reader = SimpleDirectoryReader(directory_path, recursive=True)
    documents = reader.load_data()
    return documents

def embed_text(text: str, model="text-embedding-3-small") -> np.ndarray:
    embedding = client.embeddings.create(input=[text], model=model)
    return np.array(embedding.data[0].embedding)

def process_embedding(embedding: str):
    embedding = embedding[1:-1]
    return np.fromstring(embedding, dtype=np.float64, sep=',')

def retrieve_similar_chunks(query_embedding: np.ndarray, top_k: int = 3):
    cur.execute("SELECT document_id, chunk_text, embedding FROM chunks")
    rows = cur.fetchall()
    similarities = []
    for document_id, chunk_text, embedding in rows:
        emb = process_embedding(embedding)
        similarity = cosine_similarity([query_embedding], [emb])[0][0]
        similarities.append({'document_id': document_id, 'chunk_text': chunk_text, 'similarity': similarity})
    sorted_similarities = sorted(similarities, key=lambda x: x["similarity"], reverse=True)
    return sorted_similarities[:top_k]

#TODO: better system message (from test_backend.py?)
def generate_answer(question: str, context: str) -> str:
    prompt = f"Context:\n{context}\n\nQuestion:\n{question}\n\nAnswer:"
    response = client.chat.completions.create(
        model="gpt-4o-mini",
        messages=[
            {"role": "system", "content": "You are a helpful assistant."},
            {"role": "user", "content": prompt}
        ],
        temperature=0.1
    )
    return response.choices[0].message.content



# init_db()

documents = load_documents(r"C:\OpenAI\embedding_data")

# process_and_store_docs(documents)

prompt_question = "Milyen mezői vannak az OTP SZÉP KÁRTYA igénylőlapnak?"
query_embedding = embed_text(prompt_question)
similar_chunks = retrieve_similar_chunks(query_embedding)
# print(similar_chunks)

context = "\n".join([i["chunk_text"] for i in similar_chunks])
answer = generate_answer(prompt_question, context)
print(answer)


cur.close()
conn.close()