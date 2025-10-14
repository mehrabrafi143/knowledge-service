import httpx
import logging
from typing import List
from app.models import KnowledgeEntry

logger = logging.getLogger(__name__)

class SearchService:
    def __init__(self):
        self.dotnet_base_url = "https://localhost:7254"
        self.timeout = 30.0

    async def search_entries(self, query: str, limit: int = 10) -> List[KnowledgeEntry]:
        """
        Calls .NET API and performs client-side keyword search.
        """
        try:
            async with httpx.AsyncClient(timeout=self.timeout, verify=False) as client:
                # Call .NET API to get all entries
                response = await client.get(f"{self.dotnet_base_url}/api/entries")
                response.raise_for_status()

                entries = response.json()
                query = query.lower()

                # Filter and rank results
                filtered = [
                    e for e in entries
                    if query in e.get("title", "").lower()
                    or query in e.get("description", "").lower()
                    or any(query in t.lower() for t in e.get("tags", []))
                ]

                # Simple relevance scoring
                sorted_entries = sorted(
                    filtered,
                    key=lambda e: (
                        3 * int(query in e.get("title", "").lower()) +
                        2 * int(query in e.get("description", "").lower()) +
                        1 * any(query in t.lower() for t in e.get("tags", []))
                    ),
                    reverse=True
                )[:limit]

                logger.info(f"Found {len(sorted_entries)} entries for query '{query}'")

                return [KnowledgeEntry(**e) for e in sorted_entries]

        except httpx.RequestError as e:
            logger.error(f"Could not reach .NET API: {e}")
            raise Exception("Connection to .NET API failed")
        except Exception as e:
            logger.error(f"Unexpected error during search: {e}")
            raise
