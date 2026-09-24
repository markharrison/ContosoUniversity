# Contoso University web application

This directory contains the ASP.NET Core MVC application targeting .NET 10.

## Application structure

```text
ContosoUniversity/
├── Controllers/       # MVC controllers
├── Data/              # EF Core context and seed data
├── Models/            # Entity and view models
├── Services/          # In-process notification service
├── Views/             # Razor views
├── Content/           # Published stylesheets
├── Scripts/           # Published client scripts
├── Program.cs         # Application startup and provider selection
├── appsettings.json   # Local SQLite configuration
└── ContosoUniversity.csproj
```

## Database providers

SQLite is the default provider for local development and testing. The database
is created and seeded automatically at startup.

Azure sets `DatabaseProvider=SqlServer` and supplies
`ConnectionStrings__DefaultConnection` through App Service configuration to use
Azure SQL Database.

## Run

From the repository root:

```bash
dotnet run --project ContosoUniversity/ContosoUniversity.csproj
```

For complete setup, testing, Azure deployment, and migration information, see:

- [Repository overview](../README.md)
- [Deployment guide](../deployment.md)
- [Migration notes](../migration.md)
