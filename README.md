# 🏠 HouseBrokerApp

A clean-architecture-based .NET Full-Stack Broker Application with user roles (Broker, Seeker), property listing management, image uploads, and authentication/authorization using Identity and JWT.

---

## 🧱 Tech Stack

- **Backend:** ASP.NET Core Web API  
- **Frontend:** Blazor Server  
- **Authentication:** ASP.NET Core Identity + JWT  
- **Database:** SQL Server + Entity Framework Core  
- **Architecture:** Clean Architecture (Domain, Application, Infrastructure, API)  
- **Validation:** FluentValidation  
- **Logging:** Serilog  
- **Image Uploads:** Stored in `wwwroot/Images`

---

## 📂 Folder Structure

```
HouseBrokerApp/
│
├── src/
│   ├── HouseBrokerApp.Domain/            # Domain Entities & Enums
│   ├── HouseBrokerApp.Application/       # Interfaces, Services, Validators
│   ├── HouseBrokerApp.Infrastructure/    # EF Core, DbContext, JWT, Repositories
│   ├── HouseBrokerApp.API/               # Controllers, Middleware, DI, Program.cs
│   └── HouseBrokerApp.UI/                # Blazor Server UI (Broker/Seeker Dashboards)
│
└── README.md
```

---

## 🚀 Features

- ✅ User Registration/Login (Broker & Seeker roles)
- ✅ JWT Token-Based Authentication
- ✅ Role-Based Authorization
- ✅ Property Listing CRUD
- ✅ Upload & Display Multiple Property Images
- ✅ Commission Calculation Logic (Based on Price Slabs)
- ✅ Broker-only Dashboard for Listing Management
- ✅ Search Properties by Location, Price, and Type
- ✅ FluentValidation for Model Validation
- ✅ Centralized Error Handling Middleware

---

## ⚙️ Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/HouseBrokerApp.git
cd HouseBrokerApp
```

### 2. Set up the Database

- Update the `DefaultConnection` string in `appsettings.json`
- Run EF Core Migrations:

```bash
cd src/HouseBrokerApp.API
dotnet ef database update
```

### 3. Run the Application

```bash
dotnet run --project src/HouseBrokerApp.API
```

### 4. Swagger UI

Visit: [http://localhost:5000/swagger](http://localhost:5000/swagger)  
You can test endpoints and provide JWT tokens for authorization.

---

## 🧪 Unit Testing

- Using NUnit and Moq  
- Located in `HouseBrokerApp.Tests`  
- Run with:

```bash
dotnet test
```


## 🙌 Author

**Season Banjade**  
Feel free to reach out on GitHub or [LinkedIn](#)

---

_Last updated: July 04, 2025_
