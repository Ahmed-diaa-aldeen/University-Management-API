# 🎓 University Management System API

A comprehensive, production-ready **RESTful Web API** built with **ASP.NET Core 8**, designed using **Clean Architecture**, **Repository Pattern**, and **Unit of Work Pattern**.

---

## 🛠️ Architecture & Key Features

* **Clean N-Tier Architecture:** Clear separation of concerns utilizing Repository and Unit of Work patterns for clean data persistence and maintainability.
* **Database & ORM:** SQL Server integrated with Entity Framework Core 8 (Code-First Workflow).
* **Security & Authorization:** JWT (JSON Web Token) Authentication with Role-Based Access Control (`Admin` & `Student` Roles).
* **Dynamic Query Optimization:** Server-side filtering using LINQ Expressions with dynamic `FindAsync`.
* **File Management Service:** Secure profile picture uploading and static file hosting inside `wwwroot/uploads`.
* **Academic Engine:** Self-healing student profile management, course enrollment, and weighted cumulative GPA calculations.

---

## 🚀 Technologies & Libraries Used

* **Framework:** .NET 8 (ASP.NET Core Web API)
* **Language:** C# 12
* **Database & ORM:** Microsoft SQL Server & Entity Framework Core 8
* **Authentication:** `System.IdentityModel.Tokens.Jwt` & `Microsoft.AspNetCore.Authentication.JwtBearer`

---

## 💻 How to Run Locally

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQL Server & SSMS (SQL Server Management Studio)

### Installation Steps

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/Ahmed-diaa-aldeen/University-Management-API.git](https://github.com/Ahmed-diaa-aldeen/University-Management-API.git)