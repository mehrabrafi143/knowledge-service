===========================================================
                 KNOWLEDGE SERVICE
    Hybrid .NET & FastAPI Microservices
===========================================================

This repository contains a HYBRID MICROSERVICE SYSTEM built with:

- **.NET 8 Web API** — Knowledge Service (data storage & management)
- **FastAPI (Python)** — Search Service (smart search & keyword-based filtering)

The system is designed for SCALABILITY and LANGUAGE INTEROPERABILITY, enabling seamless communication between the Python FastAPI layer and the .NET API.

-----------------------------------------------------------
OVERVIEW
-----------------------------------------------------------

.NET Web API (knowledge-service)
--------------------------------
- Acts as the **PRIMARY DATA PROVIDER**
- Exposes **RESTFUL ENDPOINTS** for knowledge entries
- Uses **SQL SERVER** as the database backend (can switch to PostgreSQL easily)
- Supports **EF CORE MIGRATIONS** and **UNIT TESTING**

FastAPI Service (fastapi-service)
---------------------------------
- Acts as a **SEARCH MICROSERVICE**, consuming data from the .NET API
- Implements **KEYWORD-BASED SEARCH** and **SIMPLE RELEVANCE SCORING**
- Supports **CORS** and can be deployed independently
- Includes **AUTOMATED TESTS** using pytest

-----------------------------------------------------------
PROJECT STRUCTURE
-----------------------------------------------------------

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

-----------------------------------------------------------
.NET WEB API SETUP
-----------------------------------------------------------

1. OPEN IN VISUAL STUDIO
Open the solution file:
D:\knowledge-service\.net-service\knowledge-service\knowledge-service.sln

2. CONFIGURE DATABASE (SQL SERVER)
Edit appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=KnowledgeDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

**Tip:** To switch to PostgreSQL, replace:
- UseSqlServer() → UseNpgsql() in Program.cs
- Update the connection string accordingly

3. ADD & APPLY EF CORE MIGRATIONS
From Visual Studio Package Manager Console:

Add-Migration InitialCreate
Update-Database

4. RUN THE .NET WEB API
Press Ctrl + F5 → Start Without Debugging

Runs at:
https://localhost:7254

Swagger UI available at:
https://localhost:7254/swagger

-----------------------------------------------------------
.NET TESTS
-----------------------------------------------------------

To run all tests:
1. Right-click the **Test Project** in Visual Studio
2. Select **Run All Tests**

Results appear in **Test Explorer** if successful

-----------------------------------------------------------
FASTAPI SETUP
-----------------------------------------------------------

1. INSTALL DEPENDENCIES
pip install -r requirements.txt

2. RUN THE FASTAPI APP
py -m uvicorn app.main:app --host 0.0.0.0 --port 8000 --reload

The app will be available at:
http://127.0.0.1:8000

Interactive docs:
http://127.0.0.1:8000/docs

-----------------------------------------------------------
FASTAPI SERVICE DETAILS
-----------------------------------------------------------

- Communicates with the .NET API at **https://localhost:7254**
- Implements **GET /search?query=keyword**
- Returns a **JSON list** of matching knowledge entries
- Uses **httpx.AsyncClient** for async HTTP calls

FASTAPI TESTS
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

-----------------------------------------------------------
TECH STACK
-----------------------------------------------------------

Layer            | Framework       | Language      | Description
-----------------|----------------|--------------|--------------------------------------
Backend API      | ASP.NET Core 8 | C#           | Core business logic & data access
Search Service   | FastAPI        | Python 3.11+ | Keyword-based search & filtering
ORM              | EF Core        | C#           | Database access & migrations
Database         | SQL Server     | —            | Relational storage
Tests            | xUnit / pytest | C# / Python  | Unit tests for both services

-----------------------------------------------------------
INTEGRATION SUMMARY
-----------------------------------------------------------

The FastAPI microservice fetches data from:
https://localhost:7254/api/entries

Then performs:
- **Keyword matching**
- **Relevance-based ranking**
- Returns the **filtered JSON list** to the client

-----------------------------------------------------------
FUTURE IMPROVEMENTS
-----------------------------------------------------------

- Move from SQL Server → PostgreSQL (minimal code change)
- Add caching (Redis / MemoryCache)
- Implement JWT authentication between services
- Add Docker Compose for unified startup

-----------------------------------------------------------
AUTHOR
-----------------------------------------------------------

**Mehrab Karim**
