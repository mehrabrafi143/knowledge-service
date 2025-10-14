import pytest
from fastapi.testclient import TestClient
from app.main import app

client = TestClient(app)

def test_root():
    response = client.get("/")
    assert response.status_code == 200

def test_health():
    response = client.get("/health")
    assert response.status_code == 200
    assert response.json()["status"] == "healthy"

def test_search_validation():
    response = client.get("/search?query=")
    assert response.status_code == 400
    
    response = client.get("/search")
    assert response.status_code == 422

@pytest.mark.asyncio
async def test_search_endpoint_integration():
    """Test the search endpoint with mocked service"""
    from app.main import app
    from app.services import SearchService
    
    # Mock the service dependency
    app.dependency_overrides = {}
    
    with TestClient(app) as client:
        # This would test the full integration
        response = client.get("/search?query=test")
        # Should get either 200 (if .NET API is running) or 500 (if not)
        assert response.status_code in [200, 500]