# Migration notes

## Completed

- Converted the application from ASP.NET MVC 5 on .NET Framework to ASP.NET
  Core MVC on .NET 10.
- Replaced `packages.config`, `Web.config`, `Global.asax`, MVC bundling, and
  controller-owned database contexts with SDK-style package references,
  `appsettings.json`, `Program.cs`, static-file middleware, and dependency
  injection.
- Updated Entity Framework Core to 10.0.11.
- Added SQLite for local development and tests and Azure SQL for Production.
- Added integration coverage for `/`, `/Home/About`, `/Students`, `/Courses`,
  `/Instructors`, and `/Departments`, including checks for seeded data.
- Added Bicep and OIDC-authenticated deploy/delete workflows.

## Further attention

### Notifications

MSMQ (`System.Messaging`) is Windows-only and cannot run on .NET 10 Linux App
Service. It was replaced with a thread-safe in-process queue so the existing
notification UI continues to work on one application instance. Messages are
not durable and are not shared across scaled-out instances. Before scaling,
replace `NotificationService` with Azure Service Bus or another durable
transport.

### Teaching-material uploads

Uploads still use the application's local `Uploads/TeachingMaterials`
directory. App Service storage is not an appropriate durable, scale-out file
store. Move these files to Azure Blob Storage before enabling multiple
instances or relying on uploads as permanent records.

### Database lifecycle

Startup currently uses `EnsureCreated` to preserve the original automatic
schema-and-seed behavior. Introduce checked-in EF Core migrations and run them
as a controlled deployment step before making production schema changes.
SQLite also does not provide SQL Server's native row-version semantics, so
department concurrency behavior should receive provider-specific tests before
it is expanded.

### Authentication

The old project was configured for IIS Windows Authentication but did not
contain application authorization logic. Windows Authentication is not enabled
in the Linux App Service deployment. If sign-in is required, add Microsoft
Identity Web with an Entra app registration and define authorization policies
before exposing administrative create, edit, and delete actions publicly.
