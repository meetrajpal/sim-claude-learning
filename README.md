# AspNetCore.MCP

A learning project that explores how to build a production-style **ASP.NET Core Web API** that doubles as a **Model Context Protocol (MCP) server** — letting Claude Code interact with it directly using natural language. The project also demonstrates how to use advanced Claude Code features: custom skills, hooks, slash commands, and project settings.

---

## What the Project Does

The application manages **Employees** and **Departments** through a layered .NET architecture. Instead of a traditional REST API client, the primary consumer is Claude Code itself via MCP. You can ask Claude things like:

> "Create a new employee named Alice in the Engineering department with a salary of 80,000."
> "Show me all active employees."
> "Delete department with ID ..."

Claude calls the MCP tools exposed by this server, which run through full business logic, validation, and database persistence — no manual API calls needed.

---

## Architecture

The solution is split into four projects following a clean layered design:

```
AspNetCore.MCP/
├── AspNetCore.MCP.Domain/        # Entities, DTOs, interfaces — no dependencies
├── AspNetCore.MCP.DAL/           # EF Core, repositories, migrations
├── AspNetCore.MCP.BAL/           # Services, validators, mappers
└── AspNetCore.MCP.Tools/         # ASP.NET Core API + MCP server entry point
```

| Layer | Responsibility |
|---|---|
| **Domain** | `Employee`, `Department`, `LeaveRequest` entities; request/response DTOs; repository and service interfaces |
| **DAL** | `ApplicationDbContext`, EF Core configurations, `BaseRepository<T>`, `UnitOfWork` |
| **BAL** | `EmployeeService`, `DepartmentService`, FluentValidation validators, Mapperly mappers |
| **Tools** | `Program.cs`, MCP tool classes, exception middleware, file logger |

### Key Design Decisions

- **Audit Tracking** — `ApplicationDbContext.SaveChangesAsync()` automatically sets `CreatedAt`/`UpdatedAt` for any entity implementing `IAuditEnitity`.
- **Soft Delete** — Entities have an `IsActive` flag (default `true`). Deletes set it to `false` rather than removing the row.
- **Sequential GUIDs** — IDs use SQL Server's `NEWSEQUENTIALID()` for clustered-index friendliness.
- **`ApiResponse<T>` wrapper** — All service methods return a typed success/failure envelope; exception middleware converts thrown exceptions to the same shape.
- **Source-generated mapping** — [Mapperly](https://github.com/riok/mapperly) generates DTO ↔ entity converters at compile time.

---

## MCP Server

This is the centrepiece of the project. The `AspNetCore.MCP.Tools` project hosts an MCP server over **HTTP transport** on `http://localhost:5021/`. Claude Code connects to it via `.mcp.json` at the project root:

```json
{
  "mcpServers": {
    "employee-mcp-server": {
      "type": "http",
      "url": "http://localhost:5021/"
    }
  }
}
```

### How It Works

Tools are plain C# classes decorated with `[McpServerToolType]`. Methods on those classes become individual MCP tools, auto-discovered at startup via `WithToolsFromAssembly()`:

```csharp
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

app.MapMcp();
```

No controller inheritance. No `IActionResult`. Each method returns `Task<string>` (serialized JSON). Exceptions bubble up and are caught by `ExceptionMiddleware`, which maps them to appropriate HTTP status codes.

### Exposed Tools

**Department tools** (`DepartmentTool`):

| Tool | Parameters | Description |
|---|---|---|
| `GetAllDepartmentsAsync` | `id?`, `isActive?`, `page`, `limit` | List departments with optional filtering and pagination |
| `CreateDepartment` | `name` | Create a new department |
| `UpdateDepartment` | `id`, `name` | Rename a department |
| `DeleteDepartment` | `id` | Soft-delete a department |

**Employee tools** (`EmployeeTool`):

| Tool | Parameters | Description |
|---|---|---|
| `GetAllEmployeesAsync` | `id?`, `isActive?`, `page`, `limit` | List employees with optional filtering and pagination |
| `CreateEmployee` | `employeeName`, `salary`, `emailId`, `joiningDate`, `departmentId`, `status` | Create a new employee |
| `UpdateEmployee` | `id`, `employeeName?`, `salary?`, `emailId?`, `joiningDate?`, `departmentId?`, `status?` | Update employee fields |
| `DeleteEmployee` | `id` | Soft-delete an employee |

### MCP Tool Coding Conventions

Captured in `.claude/skills/mcp-conventions.md` and enforced via the `/review-custom-mcp-tool` command:

- Classes: `[McpServerToolType]`, no controller inheritance
- Methods: return `Task<string>`, result via `JsonSerializer.Serialize(...)`
- Parameters: all decorated with `[Description("...")]`, complex DTOs flattened to primitives
- GUIDs: accepted as `string`, parsed with `Guid.TryParse()` — invalid value throws `ArgumentException`
- Dates: accepted as `string`, parsed with `DateOnly.Parse()`
- Errors: throw domain exceptions (`ArgumentException`, `ValidationException`, `KeyNotFoundException`) — never return HTTP result objects

---

## Claude Code Integration

This project uses four Claude Code features to improve the development experience.

### 1. CLAUDE.md

The `CLAUDE.md` file at the project root is loaded by Claude Code automatically at the start of every session. It documents:

- How to build and run the project
- The full architecture overview
- Step-by-step guides for common tasks (adding an entity, running migrations)
- Where custom skills live

This means Claude always has accurate, project-specific context without needing to rediscover it each time.

---

### 2. Custom Skills

Skills live in `.claude/skills/` and are reusable prompt modules that Claude can invoke by name. This project has three:

#### `mcp-conventions.md`
Documents the strict coding rules that all MCP tool classes must follow — attributes, return types, parameter patterns, error handling, and naming. Claude references this when writing or reviewing tool code.

#### `new-entity-checklist.md`
A 12-step checklist for adding a new entity end-to-end: entity class → EF configuration → migration → DTOs → repository → service → validators → MCP tool → DI registration. Ensures nothing gets missed.

#### `project-structure.md`
A quick-reference map of the solution layers, key files, and naming conventions (`{Entity}Tool.cs`, `I{Entity}Service.cs`, `{Entity}CreateRequestDTO.cs`, etc.).

**Usage:** Ask Claude "follow the new entity checklist to add a LeaveRequest entity" and it will work through each step in order using the stored skill context.

---

### 3. Hooks

Hooks are shell commands that Claude Code runs automatically before or after tool calls. They are configured in `.claude/settings.json`:

```json
{
  "hooks": {
    "PreToolUse": [
      {
        "matcher": "employee-mcp-server",
        "hooks": [{ "type": "command", "command": "node scripts/pre-tool-log.js" }]
      }
    ],
    "PostToolUse": [
      {
        "matcher": "employee-mcp-server",
        "hooks": [{ "type": "command", "command": "node scripts/post-tool-log.js" }]
      }
    ]
  }
}
```

The `matcher` field means hooks only fire for tools on the `employee-mcp-server` MCP server.

#### `scripts/pre-tool-log.js`
Runs **before** each MCP tool call. Reads the tool name, input, and session ID from stdin and appends a structured entry to `logs/mcp-audit.log`:
```
[2026-06-17T11:03:43.247Z] PRE | Tool: mcp__employee-mcp-server__get_all_employees | Input: {} | Session: 735f2a77...
```

#### `scripts/post-tool-log.js`
Runs **after** each MCP tool call. Captures the tool result and writes it to `logs/mcp-results.log`:
```
[2026-06-17T11:03:43.443Z] POST | Tool: mcp__employee-mcp-server__get_all_employees | Result: {...} | Session: 735f2a77...
```

Both scripts exit with code `0` (allow). A non-zero exit would block the tool call — useful for implementing access control or input validation at the hook level.

---

### 4. Custom Slash Commands

Slash commands live in `.claude/commands/` as markdown files. They appear in Claude Code as `/command-name` and send the file's content as a prompt with optional arguments.

#### `/audit-logs`
Defined in `.claude/commands/audit-logs.md`. Instructs Claude to read both log files and report:
1. Total number of tool calls made
2. Most frequently called tools
3. Tools called with empty inputs
4. Timeline of calls in the current session
5. Any suspicious patterns (excessive deletes, repeated failures)

**Usage:** Type `/audit-logs` in Claude Code to get an instant usage report without writing any analysis yourself.

#### `/review-custom-mcp-tool`
Defined in `.claude/commands/review-custom-mcp-tool.md`. Takes a file path as `$ARGUMENTS` and reviews that MCP tool class for compliance with the conventions defined in the `mcp-conventions` skill:
1. Correct `[McpServerTool]` and `[Description]` attributes
2. Meaningful parameter descriptions
3. Flat parameter mapping from DTO
4. `ArgumentException` (not `BadRequest`) for invalid GUIDs
5. `JsonSerializer.Serialize(result)` return pattern
6. No controller inheritance
7. No `IActionResult` return types

**Usage:** `/review-custom-mcp-tool AspNetCore.MCP.Tools/Tools/EmployeeTool.cs`

---

### 5. Settings and Permissions

**`.claude/settings.json`** — Project-level settings checked into source control. Contains the hooks configuration above.

**`.claude/settings.local.json`** — Machine-local overrides, not checked in. Contains:

```json
{
  "permissions": {
    "allow": [
      "mcp__employee-mcp-server__get_all_employees",
      "mcp__employee-mcp-server__update_employee"
    ]
  },
  "enabledMcpjsonServers": ["my-mcp-server", "employee-mcp-server"]
}
```

- **`permissions.allow`** — These tools run without a confirmation prompt. Read-heavy and safe update operations are pre-approved; destructive operations (delete, create) still prompt.
- **`enabledMcpjsonServers`** — Controls which servers defined in `.mcp.json` are active for this session.

---

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- Node.js (for hook scripts)

### Configure the Database

Edit `AspNetCore.MCP.Tools/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=your_db;User Id=your_user;Password=your_password;TrustServerCertificate=True;"
  }
}
```

### Apply Migrations

```bash
dotnet ef database update --project AspNetCore.MCP.DAL --startup-project AspNetCore.MCP.Tools
```

### Build and Run

```bash
dotnet build
cd AspNetCore.MCP.Tools
dotnet run
```

The API starts at:
- HTTP: `http://localhost:5021`
- HTTPS: `https://localhost:7275`
- Debug route list: `http://localhost:5021/debug/routes`

### Connect Claude Code

With the server running, open this project in Claude Code. The `.mcp.json` file is picked up automatically. Claude will have access to all employee and department tools immediately.

---

## Adding a New Entity

Follow the skill in `.claude/skills/new-entity-checklist.md` or tell Claude:

> "Follow the new entity checklist to add a LeaveRequest entity with fields for employeeId, startDate, endDate, leaveType, and status."

Claude will work through all 12 steps: entity class, EF configuration, migration, DTOs, repository interface and implementation, service interface and implementation, validators, MCP tool, and DI registration.

---

## Project File Map

```
.
├── .claude/
│   ├── settings.json               # Hooks: PreToolUse / PostToolUse
│   ├── settings.local.json         # Permissions + enabled MCP servers (local only)
│   ├── commands/
│   │   ├── audit-logs.md           # /audit-logs slash command
│   │   └── review-custom-mcp-tool.md  # /review-custom-mcp-tool slash command
│   └── skills/
│       ├── mcp-conventions.md      # MCP tool coding rules
│       ├── new-entity-checklist.md # Step-by-step entity addition guide
│       └── project-structure.md    # Layer map and naming conventions
├── scripts/
│   ├── pre-tool-log.js             # PreToolUse hook — writes to mcp-audit.log
│   └── post-tool-log.js            # PostToolUse hook — writes to mcp-results.log
├── logs/
│   ├── mcp-audit.log               # Pre-tool invocation log
│   ├── mcp-results.log             # Post-tool result log
│   └── app-YYYY-MM-DD.log          # Application log (FileLogger)
├── .mcp.json                       # MCP server connection config
├── CLAUDE.md                       # Claude Code project instructions
└── AspNetCore.MCP.slnx             # Solution file
```
