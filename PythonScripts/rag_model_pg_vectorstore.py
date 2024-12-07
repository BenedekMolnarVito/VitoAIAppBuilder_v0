import psycopg2
from llama_index.core import SimpleDirectoryReader, StorageContext
from llama_index.core import VectorStoreIndex
from llama_index.vector_stores.postgres import PGVectorStore
from sqlalchemy import make_url
# import textwrap
# import openai

db_name = "test_vector_db"
connection_string = "postgresql://vito-ai:vito1234@localhost:5432/RAGTest"
conn = psycopg2.connect(connection_string)
conn.autocommit = True

def init_db(db_name, conn):
    with conn.cursor() as c:
        c.execute("SELECT 1 FROM pg_catalog.pg_database WHERE datname = %s", (db_name,))
        exists = c.fetchone()
        if not exists:
            c.execute(f"CREATE DATABASE {db_name}")
            conn.commit()

init_db(db_name, conn)

url = make_url(connection_string)
vector_store = PGVectorStore.from_params(
    database=db_name,
    host=url.host,
    password=url.password,
    port=url.port,
    user=url.username,
    table_name="document_embeddings",
    embed_dim=1536,  # openai embedding dimension
    hnsw_kwargs={
        "hnsw_m": 16,
        "hnsw_ef_construction": 64,
        "hnsw_ef_search": 40,
        "hnsw_dist_method": "vector_cosine_ops",
    },
)

storage_context = StorageContext.from_defaults(vector_store=vector_store)
index = VectorStoreIndex.from_vector_store(vector_store=vector_store)
documents = SimpleDirectoryReader(r"C:\OpenAI\embedding_data").load_data() 
# existing_documents = index.get_all_documents()

# if set(documents) != set(existing_documents):
#     index = VectorStoreIndex.from_documents(documents, storage_context=storage_context)

query_engine = index.as_query_engine()

prompt_question = ""

response = query_engine.query(prompt_question)
print(response)





