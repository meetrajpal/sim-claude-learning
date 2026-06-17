# Project Structure

## Solution: AspNetCore.MCP

## Layers
- Tools/          → MCP entry points (replaces Controllers)
- Services/       → Business logic interfaces + implementations
- Repositories/   → Data access interfaces + implementations  
- DTOs/           → Request/Response data transfer objects
- Entities/       → Database entity models
- Middleware/      → ExceptionMiddleware, Auth etc.
- Validators/     → FluentValidation validators

## Key Files
- Program.cs              → DI registration, middleware, MapMcp()
- appsettings.json        → Connection strings, config
- .claude/settings.json   → Hooks configuration
- .claude/commands/       → Slash commands

## Naming Conventions
- Tools:        {Entity}Tool.cs
- Services:     I{Entity}Service.cs / {Entity}Service.cs
- Repositories: I{Entity}Repository.cs / {Entity}Repository.cs
- DTOs:         {Entity}{Create/Update}RequestDTO.cs
- Validators:   {Entity}{Create/Update}Validator.cs
- Entities:     {Entity}.cs

## Current Entities
- Employee (EmployeeTool, IEmployeeService, EmployeeRepository)
- Department (DepartmentTool, IDepartmentService, DepartmentRepository)