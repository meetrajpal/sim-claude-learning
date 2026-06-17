namespace AspNetCore.MCP.Domain.Entities;

public abstract class BaseEntity : IBaseEntity
{
    public virtual Guid Id { get; set; }

    public virtual bool IsActive { get; set; } = true;
}
