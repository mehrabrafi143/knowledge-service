Knowledge Service – Hybrid .NET & FastAPI Microservices

This repository contains a hybrid microservice system built with:

- .NET 8 Web API — Knowledge Service (data storage & management)
- FastAPI (Python) — Search Service (smart search & keyword-based filtering)

The system is designed for scalability and language interoperability, enabling seamless communication between the Python FastAPI layer and the .NET API.

---

Overview

.NET Web API (knowledge-service)
- Acts as the primary data provider
- Exposes RESTful endpoints for knowledge entries
- Uses SQL Server as the database backend (can switch to PostgreSQL easily)
- Supports EF Core migrations and unit testing

FastAPI Service (fastapi-service)
- Acts as a search microservice, consuming data from the .NET API
- Implements keyword-based search and simple relevance scoring
- Supports CORS and can be deployed independently
- Includes automated tests using pytest

---

Project Structure

D:\knowledge-service
|
├── .net-service/
│   └── knowledge-service/
│       ├── knowledge-service.sln
│       ├── appsettings.json
│       ├── Controllers/
│       ├── Models/
│       ├── Data/
│       ├── Services/
│       ├── Tests/
│       └── ...
|
└── fastapi-service/
    ├── app/
    │   ├── main.py
    │   ├── models.py
    │   ├── services.py
    │   └── ...
    ├── test/
    │   ├── test_main.py
    │   └── test_services.py
    ├── requirements.txt
    └── README.md

---

.NET Web API Setup

1. Open in Visual Studio
Open the solution file:
D:\knowledge-service\.net-service\knowledge-service\knowledge-service.sln

2. Configure Database (SQL Server)
Edit appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=KnowledgeDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

To switch to PostgreSQL, replace:
- UseSqlServer() → UseNpgsql() in Program.cs
- Update the connection string accordingly

3. Add & Apply EF Core Migrations
From Visual Studio Package Manager Console:

Add-Migration InitialCreate
Update-Database

4. Run the .NET Web API
Press Ctrl + F5 → Start Without Debugging

Runs at:
https://localhost:7254

Swagger UI available at:
https://localhost:7254/swagger

---

.NET Tests
To run all tests:
1. Right-click the Test Project in Visual Studio
2. Select Run All Tests

Results appear in Test Explorer if successful

---

FastAPI Setup

1. Install Dependencies
pip install -r requirements.txt

2. Run the FastAPI App
py -m uvicorn app.main:app --host 0.0.0.0 --port 8000 --reload

The app will be available at:
http://127.0.0.1:8000

Interactive docs:
http://127.0.0.1:8000/docs

---

FastAPI Service Details

- Communicates with the .NET API at https://localhost:7254
- Implements GET /search?query=keyword
- Returns a JSON list of matching knowledge entries
- Uses httpx.AsyncClient for async HTTP calls

FastAPI Tests
Run all tests:
py -m pytest test/ -v

Sample output:
test/test_main.py::test_root PASSED
test/test_main.py::test_health PASSED
test/test_main.py::test_search_validation PASSED
test/test_main.py::test_search_endpoint_integration PASSED
test/test_services.py::TestSearchService::test_search_entries_success PASSED
test/test_services.py::TestSearchService::test_search_entries_no_results PASSED
test/test_services.py::TestSearchService::test_search_entries_network_error PASSED
test/test_services.py::TestSearchService::test_search_entries_http_error PASSED
test/test_services.py::TestSearchService::test_relevance_scoring_logic PASSED

---

Tech Stack

Layer | Framework | Language | Description
------|-----------|---------|------------
Backend API | ASP.NET Core 8 | C# | Core business logic & data access
Search Service | FastAPI | Python 3.11+ | Keyword-based search & filtering
ORM | EF Core | C# | Database access & migrations
Database | SQL Server | — | Relational storage
Tests | xUnit / pytest | C# / Python | Unit tests for both services

---

Integration Summary

The FastAPI microservice fetches data from:
https://localhost:7254/api/entries

Then performs:
- Keyword matching
- Relevance-based ranking
- Returns the filtered JSON list to the client

---

Future Improvements

- Move from SQL Server → PostgreSQL (minimal code change)
- Add caching (Redis / MemoryCache)
- Implement JWT authentication between services
- Add Docker Compose for unified startup

---

Author

Mehrab Karim
