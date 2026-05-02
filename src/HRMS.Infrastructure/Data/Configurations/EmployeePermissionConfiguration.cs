using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations;

public class EmployeePermissionConfiguration : IEntityTypeConfiguration<EmployeePermission>
{
    public void Configure(EntityTypeBuilder<EmployeePermission> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Permission).HasConversion<string>().IsRequired();
        builder.Property(p => p.GrantedAt).IsRequired();

        builder.HasIndex(p => new { p.EmployeeId, p.Permission }).IsUnique();

        builder.HasOne(p => p.Employee)
            .WithMany(e => e.GrantedPermissions)
            .HasForeignKey(p => p.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
