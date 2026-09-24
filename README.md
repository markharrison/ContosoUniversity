# Contoso University

Contoso University is a university management web application modernized from
ASP.NET MVC 5 on .NET Framework to ASP.NET Core MVC on .NET 10.

## Technology

- ASP.NET Core MVC on .NET 10
- Entity Framework Core 10
- SQLite for local development and integration tests
- Azure SQL Database for Azure deployments
- Linux Azure App Service
- Bicep infrastructure as code
- GitHub Actions deployment using Azure OIDC

## Features

- Student management with search and pagination
- Course and teaching-material management
- Instructor course assignments and office locations
- Department and administrator management
- Enrollment statistics
- In-process change notifications

## Repository structure

```text
.
├── ContosoUniversity/         # ASP.NET Core application
├── ContosoUniversity.Tests/   # Endpoint integration tests
├── infra/                     # Azure Bicep templates
├── .github/workflows/         # Deploy and delete workflows
├── deployment.md              # Local and Azure deployment guidance
└── migration.md               # Migration status and follow-up work
```

## Run locally

Install the .NET 10 SDK, then run from the repository root:

```bash
dotnet restore ContosoUniversity/ContosoUniversity.sln
dotnet test ContosoUniversity/ContosoUniversity.sln --configuration Release
dotnet run --project ContosoUniversity/ContosoUniversity.csproj
```

The application creates and seeds
`ContosoUniversity/contoso-university.db` automatically. The local SQLite
database is ignored by Git.

The application includes these primary pages:

- `/`
- `/Home/About`
- `/Students`
- `/Courses`
- `/Instructors`
- `/Departments`

## Deploy to Azure

The Bicep templates provision Azure App Service and Azure SQL Database. The
deploy and delete workflows authenticate to Azure through GitHub OIDC.

See [deployment.md](deployment.md) for prerequisites, OIDC trust configuration,
deployment, and deletion instructions.

## Migration status

The .NET 10 migration is complete for the existing web application. Durable
notifications, blob-backed teaching-material storage, controlled EF Core
migrations, and application authentication remain recommended production
enhancements.

See [migration.md](migration.md) for details.
