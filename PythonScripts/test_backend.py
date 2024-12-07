from unittest.mock import MagicMock, patch
from backend import embed_text

def test_embed_text_single_word():
    # Mock the OpenAI client
    mock_client = MagicMock()
    mock_embedding = MagicMock()
    mock_embedding.data = [MagicMock(embedding=[0.1, 0.2, 0.3])]
    mock_client.embeddings.create.return_value = mock_embedding

    # Patch the client in the module where embed_text is defined
    with patch('backend.client', mock_client):
        text = "hello"
        expected_embedding = [0.1, 0.2, 0.3]
        
        # Call the function
        result = embed_text(text)
        
        # Assertions
        mock_client.embeddings.create.assert_called_once_with(input=[text], model="text-embedding-3-small")
        assert result == expected_embedding

def test_embed_text_consistent_embeddings():
    # Mock the OpenAI client
    mock_client = MagicMock()
    mock_embedding = MagicMock()
    mock_embedding.data = [MagicMock(embedding=[0.1, 0.2, 0.3])]
    mock_client.embeddings.create.return_value = mock_embedding

    # Patch the client in the module where embed_text is defined
    with patch('backend.client', mock_client):
        text = "consistent"
        expected_embedding = [0.1, 0.2, 0.3]
        
        # Call the function twice with the same input
        result1 = embed_text(text)
        result2 = embed_text(text)
        
        # Assertions
        mock_client.embeddings.create.assert_called_with(input=[text], model="text-embedding-3-small")
        assert result1 == expected_embedding
        assert result2 == expected_embedding
        assert result1 == result2
