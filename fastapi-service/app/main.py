from fastapi import FastAPI, Depends, HTTPException
from fastapi.middleware.cors import CORSMiddleware
import logging
from typing import Optional
from app.models import SearchResponse
from app.services import SearchService

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

app = FastAPI(
    title="Knowledge Search API",
    description="FastAPI service that searches Knowledge Entries stored in .NET service",
    version="1.0.0"
)

# Enable CORS (for local testing)
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

def get_search_service():
    return SearchService()

@app.get("/")
async def root():
    return {"message": "Knowledge Search API is running"}

@app.get("/health")
async def health():
    return {"status": "healthy", "service": "FastAPI Knowledge Search"}

@app.get("/search", response_model=SearchResponse)
async def search(query: str, limit: Optional[int] = 10, service: SearchService = Depends(get_search_service)):
    if not query.strip():
        raise HTTPException(status_code=400, detail="Query parameter cannot be empty")
    try:
        results = await service.search_entries(query, limit)
        return SearchResponse(query=query, results=results, total_count=len(results))
    except Exception as e:
        logger.error(f"Error during search: {e}")
        raise HTTPException(status_code=500, detail=str(e))
