# MCP Server Coding Conventions

## Tool Rules
- All tool classes decorated with [McpServerToolType]
- No Controller inheritance
- No IActionResult return types
- All methods return Task<string>
- All results serialized with JsonSerializer.Serialize(result)

## Parameter Rules
- All parameters decorated with [Description("...")]
- Guid always accepted as string, parsed with Guid.TryParse()
- DateOnly always accepted as string, parsed with DateOnly.Parse()
- Complex DTOs flattened into primitive parameters
- Optional fields use nullable types with default null

## Error Rules
- Invalid Guid → throw ArgumentException
- Validation failure → throw ValidationException
- Not found → throw KeyNotFoundException
- Unauthorized → throw UnauthorizedAccessException
- Never return BadRequest, NotFound, Ok etc.

## Naming Rules
- Tool class: {Entity}Tool
- Tool methods: same as service methods
- Parameters: camelCase matching DTO property names