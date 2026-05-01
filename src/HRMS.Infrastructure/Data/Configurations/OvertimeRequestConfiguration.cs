using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations;

public class OvertimeRequestConfiguration : IEntityTypeConfiguration<OvertimeRequest>
{
    public void Configure(EntityTypeBuilder<OvertimeRequest> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Reason).HasMaxLength(1000);
        builder.Property(o => o.ManagerNote).HasMaxLength(500);
        builder.Property(o => o.HRNote).HasMaxLength(500);
        builder.Property(o => o.Status).HasConversion<string>();

        builder.HasQueryFilter(o => !o.IsDeleted);

        builder.HasOne(o => o.Employee)
            .WithMany(e => e.OvertimeRequests)
            .HasForeignKey(o => o.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.ManagerReviewer)
            .WithMany()
            .HasForeignKey(o => o.ManagerReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.HRReviewer)
            .WithMany()
            .HasForeignKey(o => o.HRReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
