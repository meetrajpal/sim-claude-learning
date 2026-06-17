namespace AspNetCore.MCP.DAL.Data.Configurations;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(x => x.EmployeeId).IsRequired();

        builder.Property(x => x.StartDate).HasColumnType("date").IsRequired();

        builder.Property(x => x.EndDate).HasColumnType("date").IsRequired();

        builder.Property(x => x.LeaveType).HasColumnType("nvarchar(100)").IsRequired();

        builder.Property(x => x.Status).HasColumnType("nvarchar(50)").IsRequired();

        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
