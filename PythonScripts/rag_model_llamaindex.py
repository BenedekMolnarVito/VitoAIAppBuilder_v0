import os
# from langtrace_python_sdk import langtrace
from llama_index.core import VectorStoreIndex, SimpleDirectoryReader, StorageContext
from llama_index.vector_stores.pinecone import PineconeVectorStore
from pinecone import Pinecone, ServerlessSpec

# langtrace.init(api_key=os.getenv("LANGTRACE_API_KEY"))

api_key = os.getenv("PINECONE_API_KEY")
pc = Pinecone(api_key=api_key)
# TODO: how to pass index_name as a parameter from WinForm APP?
index_name = "code-context-test"

if index_name not in pc.list_indexes().names():
    pc.create_index(
        name=index_name,
        dimension=1536,
        metric="cosine",
        spec=ServerlessSpec(cloud="aws", region="us-east-1")
    )

pinecone_index = pc.Index(index_name)
vector_store = PineconeVectorStore(pinecone_index=pinecone_index)
storage_context = StorageContext.from_defaults(vector_store=vector_store)

index = VectorStoreIndex.from_vector_store(vector_store=vector_store)
if not index:
    documents = SimpleDirectoryReader(r"C:\OpenAI\embedding_data").load_data()
    index = VectorStoreIndex.from_documents(documents, storage_context=storage_context)


query_engine = index.as_query_engine()
response = query_engine.query("How does the WinForm app AIAppBuilder_v0 btnLoad_Click function work?")
print(response)