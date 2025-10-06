# Contoso University - .NET 9.0

This project is an ASP.NET Core MVC application targeting .NET 9.0 (migrated from .NET Framework 4.8.2).

## Project Overview

### Framework
- ASP.NET Core MVC (.NET 9.0)

### Database Access: Entity Framework
- Entity Framework Core 9.0.0

### Project Structure
```
ContosoUniversity/
├── Controllers/            # MVC Controllers
├── Data/                   # Entity Framework context and initializer
├── Models/                 # Data models and view models
├── Views/                  # Razor views
├── wwwroot/               # Static files (CSS, JavaScript, images)
├── Program.cs             # Application entry point and configuration
├── appsettings.json       # Application configuration
└── ContosoUniversity.csproj # Project file
```

## Database Configuration

The application uses SQL Server LocalDB with the following connection string in `Web.config`:
The application uses SQL Server with the following connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=ContosoUniversityNoAuthEFCore;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True"
  }
}
```

**Note**: SQL Server LocalDB is only supported on Windows. For development on Linux/macOS, you can:
- Use SQL Server in a Docker container
- Use SQLite by changing the connection string and adding `Microsoft.EntityFrameworkCore.Sqlite` package
- Use a remote SQL Server instance

## Running the Application

1. **Prerequisites**:
   - .NET 9.0 SDK
   - SQL Server LocalDB (Windows) or SQL Server (all platforms)

2. **Setup**:
   ```bash
   cd ContosoUniversity
   dotnet restore
   dotnet build
   dotnet run
   ```

3. **Access the application**:
   - Navigate to `http://localhost:5000` or `https://localhost:5001`

## Features

- **Student Management**: CRUD operations for students with pagination and search
- **Course Management**: Manage courses and their assignments to departments
- **Instructor Management**: Handle instructor assignments and office locations
- **Department Management**: Manage departments and their administrators
- **Statistics**: View enrollment statistics by date
- **File Uploads**: Support for teaching material image uploads

## Database Initialization

The application uses Entity Framework Core Code First with a database initializer that:
- Creates the database if it doesn't exist
- Seeds sample data including students, instructors, courses, and departments
- Runs automatically on application startup
