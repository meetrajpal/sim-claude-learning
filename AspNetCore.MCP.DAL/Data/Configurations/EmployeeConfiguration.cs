namespace AspNetCore.MCP.DAL.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(x => x.EmployeeName).HasColumnType("nvarchar(100)").IsRequired();

        builder.Property(x => x.Status).HasColumnType("nvarchar(100)").IsRequired();

        builder.Property(x => x.Salary).HasColumnType("decimal(12, 2)").IsRequired();

        builder.Property(x => x.EmailId).HasColumnType("nvarchar(100)").IsRequired();

        builder.Property(x => x.JoiningDate).HasColumnType("date").IsRequired();

        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Department).WithMany(z => z.Employees).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
    }
}
