# Adding a New Entity Checklist

When adding a new entity (e.g. LeaveRequest), follow this order:

## Steps
1. Create Entity class in Entities/
2. Add DbSet to AppDbContext
3. Create migration: dotnet ef migrations add Add{Entity}
4. Create RequestDTOs (Create + Update) in DTOs/
5. Create IRepository interface in Repositories/
6. Implement Repository class in Repositories/
7. Create IService interface in Services/
8. Implement Service class in Services/
9. Create FluentValidation validators in Validators/
10. Create MCP Tool in Tools/ following mcp-conventions.md
11. Register in Program.cs if not using assembly scanning
12. Test via Claude Code using natural language

## Registration in Program.cs
- Repository: builder.Services.AddScoped<I{Entity}Repository, {Entity}Repository>()
- Service: builder.Services.AddScoped<I{Entity}Service, {Entity}Service>()
- Validators: builder.Services.AddScoped<IValidator<{Entity}CreateRequestDTO>, {Entity}CreateValidator>()
- Tool: auto-discovered by WithToolsFromAssembly()