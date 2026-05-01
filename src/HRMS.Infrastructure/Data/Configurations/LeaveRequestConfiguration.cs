using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder.HasKey(lr => lr.Id);
        builder.Property(lr => lr.Reason).HasMaxLength(1000);
        builder.Property(lr => lr.ManagerNote).HasMaxLength(500);
        builder.Property(lr => lr.HRNote).HasMaxLength(500);
        builder.Property(lr => lr.Status).HasConversion<string>();

        builder.HasQueryFilter(lr => !lr.IsDeleted);

        builder.HasOne(lr => lr.Employee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(lr => lr.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.ManagerReviewer)
            .WithMany()
            .HasForeignKey(lr => lr.ManagerReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.HRReviewer)
            .WithMany()
            .HasForeignKey(lr => lr.HRReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
