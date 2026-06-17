namespace AspNetCore.MCP.DAL.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>

{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(x => x.DepartmentName).HasColumnType("nvarchar(100)").IsRequired();
        builder.HasIndex(x => x.DepartmentName).HasDatabaseName("IX_Unique_Department_DepartmentName").IsUnique();

        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
    }
}
