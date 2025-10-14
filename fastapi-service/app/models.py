from pydantic import BaseModel
from datetime import datetime
from typing import List


class KnowledgeEntry(BaseModel):
    id: int
    title: str
    description: str
    tags: List[str]
    createdAt: datetime
    updatedAt: datetime


class SearchResponse(BaseModel):
    query: str
    results: List[KnowledgeEntry]
    total_count: int
