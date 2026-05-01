using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations;

public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        builder.HasKey(lt => lt.Id);
        builder.Property(lt => lt.Name).IsRequired().HasMaxLength(100);

        builder.HasQueryFilter(lt => !lt.IsDeleted);

        builder.HasOne(lt => lt.Company)
            .WithMany(c => c.LeaveTypes)
            .HasForeignKey(lt => lt.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
