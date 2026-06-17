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

        // Relationship: One Employee can have many LeaveRequests
        builder.HasOne(x => x.Employee)
            .WithMany() // Employee entity doesn't have a collection of LeaveRequests yet; we could add it but not required for now
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}