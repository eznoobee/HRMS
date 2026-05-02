using HRMS.Domain.Entities;
using HRMS.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HRMS.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.FatherName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.GrandfatherName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.FamilyName).IsRequired().HasMaxLength(100);
        var emailConverter = new ValueConverter<Email, string>(
            v => v.Value,
            v => Email.Create(v));

        builder.Property(e => e.Email)
            .HasConversion(emailConverter)
            .IsRequired()
            .HasMaxLength(256);
        builder.Property(e => e.JobTitle).HasMaxLength(150);
        builder.Property(e => e.Role).HasConversion<string>();
        builder.Property(e => e.Permissions).HasConversion<int>().HasDefaultValue(HRPermission.None);

        var phoneConverter = new ValueConverter<PhoneNumber?, string?>(
            v => v != null ? v.Value : null,
            v => !string.IsNullOrEmpty(v) ? PhoneNumber.TryCreate(v) : null);

        builder.Property(e => e.Phone)
            .HasConversion(phoneConverter)
            .HasMaxLength(20);

        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.UserId);

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasOne(e => e.Company)
            .WithMany(c => c.Employees)
            .HasForeignKey(e => e.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
