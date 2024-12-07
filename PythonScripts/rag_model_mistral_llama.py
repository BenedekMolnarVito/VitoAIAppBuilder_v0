# Install necessary libraries
# pip install llama-index mistral

from llama_index import DocumentLoader, TextSplitter, EmbeddingGenerator
from llama_index.vector_database import VectorDatabase

# Load the Mistral-7B model (quantized 4-bit version)
mistral_model = EmbeddingGenerator(model="mistral-7b-instruct-v0.2.Q4_K_M.gguf")

# Indexing Phase
def create_index(doc_path):
    # Step 1: Load Documents
    documents = DocumentLoader.load_from_directory(doc_path)

    # Step 2: Split Documents into Chunks
    splitter = TextSplitter(chunk_size=512)
    chunks = splitter.split_documents(documents)

    # Step 3: Generate Embeddings for Chunks
    embeddings = mistral_model.generate_embeddings(chunks)

    # Step 4: Store Embeddings in Vector Database
    vector_db = VectorDatabase()
    vector_db.store_embeddings(chunks, embeddings)
    return vector_db

# Retrieval Phase
def retrieve_contexts(query, vector_db, embedding_generator):
    # Generate Embedding for Query
    query_embedding = embedding_generator.generate_embedding(query)
    
    # Search Vector Database for Top Matches
    results = vector_db.search(query_embedding, top_k=5)
    return results  # Retrieved relevant document chunks

# Generation Phase
def generate_response(query, contexts, mistral_model):
    # Construct Prompt with Contexts
    prompt = f"Answer the question based on the following context:\n\n{contexts}\n\nQuestion: {query}"
    
    # Generate Response using Mistral-7B
    response = mistral_model.generate_text(prompt)
    return response

# Full RAG Pipeline
def query_pipeline(user_query, vector_db, mistral_model):
    # Retrieve contexts for the query
    contexts = retrieve_contexts(user_query, vector_db, mistral_model)
    
    # Generate response with retrieved contexts
    response = generate_response(user_query, contexts, mistral_model)
    return response

# Usage Example
if __name__ == "__main__":
    # Create the index (adjust the path to your documents)
    vector_db = create_index("path/to/docs")
    
    # Sample query to test the RAG pipeline
    user_query = "What is Retrieval-Augmented Generation?"
    response = query_pipeline(user_query, vector_db, mistral_model)
    
    print("Response:", response)
