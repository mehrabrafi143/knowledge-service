\# 🧠 Knowledge Service – Hybrid .NET \& FastAPI Microservices



This repository contains a \*\*hybrid microservice system\*\* built with:

\- \*\*.NET 8 Web API\*\* (Knowledge Service – backend for data storage \& management)

\- \*\*FastAPI (Python)\*\* (Search Service – smart search \& keyword-based filtering)



The system is designed for scalability and language interoperability, allowing the Python FastAPI layer to communicate seamlessly with the .NET API.



---



\## 🚀 Overview



\### 🔹 .NET Web API (`knowledge-service`)

\- Acts as the \*\*primary data provider\*\*.

\- Exposes RESTful endpoints for knowledge entries.

\- Uses \*\*SQL Server\*\* as the database backend (can be switched to PostgreSQL with minimal changes).

\- Supports entity migrations and unit testing.



\### 🔹 FastAPI Service (`fastapi-service`)

\- Acts as a \*\*search microservice\*\*, consuming data from the .NET API.

\- Implements keyword-based search and simple relevance scoring.

\- Supports CORS and can be independently deployed.

\- Includes automated tests using `pytest`.



---



\## 🗂️ Project Structure



D:\\knowledge-service

│

├── .net-service/

│ └── knowledge-service/

│ ├── knowledge-service.sln

│ ├── appsettings.json

│ ├── Controllers/

│ ├── Models/

│ ├── Data/

│ ├── Services/

│ ├── Tests/

│ └── ...

│

└── fastapi-service/

├── app/

│ ├── main.py

│ ├── models.py

│ ├── services.py

│ └── ...

├── test/

│ ├── test\_main.py

│ └── test\_services.py

├── requirements.txt

└── README.md



yaml

Copy code



---



\## ⚙️ .NET Web API Setup



\### 1. Open in Visual Studio

Open the solution file:

D:\\knowledge-service.net-service\\knowledge-service\\knowledge-service.sln



pgsql

Copy code



\### 2. Configure Database (SQL Server)

Edit `appsettings.json`:



```json

"ConnectionStrings": {

&nbsp; "DefaultConnection": "Server=localhost;Database=KnowledgeDb;Trusted\_Connection=True;TrustServerCertificate=True;"

}

💡 The project is configured for SQL Server by default.

To switch to PostgreSQL later, simply update:



The UseSqlServer() line in Program.cs → UseNpgsql()



And change the connection string accordingly.



3\. Add and Apply EF Core Migrations

From Visual Studio Package Manager Console:



powershell

Copy code

Add-Migration InitialCreate

Update-Database

This creates the initial schema and updates your SQL Server database.



4\. Run the .NET Web API

Press F5 or use:



sql

Copy code

Ctrl + F5 → Start Without Debugging

The service will start on:



arduino

Copy code

https://localhost:7254

You can access Swagger UI at:

👉 https://localhost:7254/swagger



🧪 .NET Tests

All tests are included in the test project within the same solution.



To run all tests:



Right-click the Test Project in Visual Studio



Select Run All Tests



✅ If all tests pass, you’ll see the result in the Test Explorer panel.



🐍 FastAPI Setup

1\. Create and Activate Virtual Environment

bash

Copy code

cd D:\\knowledge-service\\fastapi-service

python -m venv venv

source venv/Scripts/activate  # for Git Bash

2\. Install Dependencies

bash

Copy code

pip install -r requirements.txt

3\. Run FastAPI App

bash

Copy code

py -m uvicorn app.main:app --host 0.0.0.0 --port 8000 --reload

The app will be available at:



cpp

Copy code

http://127.0.0.1:8000

Interactive docs:

👉 http://127.0.0.1:8000/docs



🧩 FastAPI Service Details

Communicates with the .NET API at https://localhost:7254



Implements GET /search?query=keyword



Returns a JSON response of matching knowledge entries



Uses httpx.AsyncClient to fetch and process data



🧪 FastAPI Tests

Run All Tests

bash

Copy code

py -m pytest test/ -v

Sample output:



bash

Copy code

=========================== test session starts ============================

collected 9 items



test/test\_main.py::test\_health\_check PASSED

test/test\_main.py::test\_search\_endpoint PASSED

test/test\_services.py::test\_search\_service\_matches\_query FAILED

test/test\_services.py::test\_search\_service\_no\_match PASSED

...



================== 2 failed, 7 passed, 3 warnings in 0.27s ==================

⚠️ Two tests may fail due to Pydantic field naming differences (createdAt vs. created\_at), which can be easily fixed in models.py if desired.



🧱 Tech Stack

Layer	Framework	Language	Description

Backend API	ASP.NET Core 8	C#	Core business logic and database

Search Service	FastAPI	Python 3.11+	Search and data filtering

ORM	EF Core	C#	Database access and migrations

Database	SQL Server	—	Relational database

Tests	xUnit / pytest	C# / Python	Unit testing for both services



🔄 Integration Summary

The FastAPI microservice fetches data from the running .NET API at:



bash

Copy code

https://localhost:7254/api/entries

Then it performs:



Client-side keyword matching



Simple relevance-based ranking



Returns the filtered list to the client



💡 Future Improvements

Move from SQL Server to PostgreSQL (minimal code change required)



Add caching layer (Redis or MemoryCache)



Implement JWT authentication between services



Add Docker Compose for unified service startup



🧑‍💻 Author

Mehrab Karim

