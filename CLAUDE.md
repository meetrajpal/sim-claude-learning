# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Getting Started

### Prerequisites
- .NET 10.0 SDK
- SQL Server (or configure connection string accordingly)

### Building the Solution
To build the entire solution, run:
```bash
dotnet build
```

### Running the API
The API (AspNetCore.MCP.Tools) is an ASP.NET Core Web API that also hosts an MCP (Model Context Protocol) server.

To run the API:
```bash
cd AspNetCore.MCP.Tools
dotnet run
```
The API will be available at:
- HTTP: http://localhost:5021
- HTTPS: https://localhost:7275

The MCP server is available via HTTP transport (check the MCP documentation for the exact endpoint).

### Environment Configuration
The application uses a connection string named "DefaultConnection" in `appsettings.json`. By default, it is empty. You need to configure it for SQL Server:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=your_db;User Id=your_user;Password=your_password;TrustServerCertificate=True;"
  }
}
```
Alternatively, you can set the connection string via environment variables.

### Debug Endpoint
A debug endpoint is available at `/debug/routes` to list all registered endpoints.

## Architecture Overview

The solution follows a layered architecture with four main projects:

1. **AspNetCore.MCP.Domain** 
   - Contains core entities (`Employee`, `Department`), interfaces, DTOs (Data Transfer Objects), and shared models.
   - Entities inherit from `BaseEntity` and implement audit interfaces (`IAuditEnitity`).
   - DTOs are used for input/output validation and mapping.

2. **AspNetCore.MCP.DAL** (Data Access Layer)
   - Implements data access using Entity Framework Core.
   - Contains `ApplicationDbContext` with `DbSet<Department>` and `DbSet<Employee>`.
   - Entity configurations (`DepartmentConfiguration`, `EmployeeConfiguration`) define table schema, constraints, and default values.
   - Repositories implement generic and specific data access patterns.

3. **AspNetCore.MCP.BAL** (Business Access Layer)
   - Contains business logic, services, validators, and mappers.
   - Services (`DepartmentService`, `EmployeeService`) orchestrate operations using repositories and unit of work.
   - Validators (using FluentValidation) validate DTOs before processing.
   - Mappers convert between DTOs and entities.

4. **AspNetCore.MCP.Tools** (API Project)
   - ASP.NET Core Web API project targeting .NET 10.0.
   - Hosts the MCP server via `ModelContextProtocol.AspNetCore` package.
   - Tools (`DepartmentTool`, `EmployeeTool`) expose MCP server endpoints that mirror the service operations.
   - Extension methods (`ServiceCollectionExtensions`) register services, database context, and validators.
   - Middleware for exception handling.

### Key Features
- **Audit Tracking**: Entities automatically set `CreatedAt` and `UpdatedAt` via `SaveChangesAsync` override in `ApplicationDbContext`.
- **Soft Delete**: Entities have an `IsActive` property (default true) for soft delete patterns.
- **Sequential GUIDs**: Ids use `NEWSEQUENTIALID()` for better index performance in SQL Server.
- **MCP Server**: Exposes department and employee operations as MCP tools over HTTP transport.

## Custom Skills

- Check .claude\skills\ directory for custom skills made by the user for some particular task.

## Common Development Tasks

### Adding a New Entity
1. Define the entity in `AspNetCore.MCP.Domain.Entities` (inherit from `BaseEntity` and any required interfaces).
2. Add corresponding DTOs in `AspNetCore.MCP.Domain.DTOs\[Entity]` if needed.
3. Configure EF Core mapping in `AspNetCore.MCP.DAL.Data.Configurations` (implement `IEntityTypeConfiguration<T>`).
4. Add `DbSet<T>` to `ApplicationDbContext`.
5. Add repository interface in `AspNetCore.MCP.Domain.Interfaces.Repositories` and implementation in `AspNetCore.MCP.DAL.Repositories`.
6. Add service interface in `AspNetCore.MCP.Domain.Interfaces.Services` and implementation in `AspNetCore.MCP.BAL.Services`.
7. Add mapper in `AspNetCore.MCP.BAL.Mappers` (and interface if needed).
8. Add validators in `AspNetCore.MCP.BAL.Validators\[Entity]`.
9. Register the new repository, service, mapper, and validator in `ServiceCollectionExtensions.AddApplicationServices`.
10. Expose the entity via MCP tool by creating a new tool class in `AspNetCore.MCP.Tools.Tools` (similar to `DepartmentTool`/`EmployeeTool`).

### Running Tests
Currently, there are no test projects in the solution. To add tests:
1. Create a new test project (e.g., `AspNetCore.MCP.Tests`) using `xunit` or `NUnit`.
2. Add references to the projects under test.
3. Write unit tests for services, validators, and controllers/tools.

### Database Migrations
The solution uses Entity Framework Core migrations.
- To create a new migration: `dotnet ef migrations add <MigrationName> --project AspNetCore.MCP.DAL --startup-project AspNetCore.MCP.Tools`
- To apply migrations: `dotnet ef database update --project AspNetCore.MCP.DAL --startup-project AspNetCore.MCP.Tools`

### MCP Server Usage
The MCP server exposes the following tools (as seen in the tool classes):
- **Department Operations**: Get all departments, create, update, delete.
- **Employee Operations**: Get all employees, create, update, delete.

These tools can be invoked by MCP clients (like Claude Code) once the server is running.

## Notes
- The solution uses .NET 10.0, so ensure the SDK is installed.
- The API uses SQL Server; other database providers would require changing the `UseSqlServer` call in `ServiceCollectionExtensions.AddDatabase`.
- Exception handling is centralized via `ExceptionMiddleware`.
- Logging is implemented through `IFileLogger` (writes to files in the `logs` directory).
