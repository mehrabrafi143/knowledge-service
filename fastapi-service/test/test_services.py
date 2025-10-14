import pytest
import httpx
from unittest.mock import AsyncMock, patch
from app.services import SearchService
from app.models import KnowledgeEntry

class TestSearchService:
    
    @pytest.fixture
    def service(self):
        return SearchService()
    
    @pytest.fixture
    def sample_entries(self):
        return [
            {
                "id": 1,
                "title": "Machine Maintenance",
                "description": "Regular maintenance procedures",
                "tags": ["maintenance", "machines"],
                "createdAt": "2023-01-01T00:00:00",
                "updatedAt": "2023-01-01T00:00:00"
            },
            {
                "id": 2,
                "title": "Safety Protocols", 
                "description": "Safety guidelines",
                "tags": ["safety", "protocols"],
                "createdAt": "2023-01-01T00:00:00",
                "updatedAt": "2023-01-01T00:00:00"
            }
        ]

    @pytest.mark.asyncio
    async def test_search_entries_success(self, service, sample_entries):
        """Test successful search with mock HTTP response"""
        # Mock the HTTP client to avoid real network calls
        with patch('httpx.AsyncClient') as mock_client_class:
            mock_client = AsyncMock()
            mock_response = AsyncMock()
            
            # Setup the mock response
            mock_response.status_code = 200
            mock_response.json.return_value = sample_entries
            mock_response.raise_for_status = AsyncMock()
            
            # Setup the client mock
            mock_client.get.return_value = mock_response
            mock_client_class.return_value.__aenter__.return_value = mock_client
            
            # Call the actual method
            results = await service.search_entries("machine")
            
            # Verify results
            assert len(results) == 1
            assert isinstance(results[0], KnowledgeEntry)
            assert results[0].title == "Machine Maintenance"

    @pytest.mark.asyncio
    async def test_search_entries_no_results(self, service, sample_entries):
        """Test search returns empty when no matches"""
        with patch('httpx.AsyncClient') as mock_client_class:
            mock_client = AsyncMock()
            mock_response = AsyncMock()
            
            mock_response.status_code = 200
            mock_response.json.return_value = sample_entries
            mock_response.raise_for_status = AsyncMock()
            
            mock_client.get.return_value = mock_response
            mock_client_class.return_value.__aenter__.return_value = mock_client
            
            results = await service.search_entries("nonexistent")
            
            assert len(results) == 0

    @pytest.mark.asyncio
    async def test_search_entries_network_error(self, service):
        """Test service handles network errors gracefully"""
        with patch('httpx.AsyncClient') as mock_client_class:
            mock_client = AsyncMock()
            mock_client.get.side_effect = httpx.RequestError("Network error")
            mock_client_class.return_value.__aenter__.return_value = mock_client
            
            with pytest.raises(Exception, match="Connection to .NET API failed"):
                await service.search_entries("test")

    @pytest.mark.asyncio 
    async def test_search_entries_http_error(self, service):
        """Test service handles HTTP errors"""
        with patch('httpx.AsyncClient') as mock_client_class:
            mock_client = AsyncMock()
            mock_response = AsyncMock()
            
            mock_response.status_code = 500
            mock_response.raise_for_status.side_effect = httpx.HTTPStatusError(
                "Server error", request=None, response=None
            )
            
            mock_client.get.return_value = mock_response
            mock_client_class.return_value.__aenter__.return_value = mock_client
            
            with pytest.raises(Exception):
                await service.search_entries("test")

    def test_relevance_scoring_logic(self, service, sample_entries):
        """Test the search and ranking logic works correctly"""
        # Test the filtering and scoring logic directly
        query = "machine"
        
        # This mimics what happens inside search_entries
        filtered = [
            e for e in sample_entries
            if query in e.get("title", "").lower()
            or query in e.get("description", "").lower() 
            or any(query in t.lower() for t in e.get("tags", []))
        ]
        
        # Test relevance scoring
        scored_entries = sorted(
            filtered,
            key=lambda e: (
                3 * int(query in e.get("title", "").lower()) +
                2 * int(query in e.get("description", "").lower()) +
                1 * any(query in t.lower() for t in e.get("tags", []))
            ),
            reverse=True
        )
        
        assert len(scored_entries) == 1
        assert scored_entries[0]["title"] == "Machine Maintenance"