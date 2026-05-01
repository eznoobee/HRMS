using HRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRMS.Infrastructure.Data.Configurations;

public class ChannelMemberConfiguration : IEntityTypeConfiguration<ChannelMember>
{
    public void Configure(EntityTypeBuilder<ChannelMember> builder)
    {
        builder.HasKey(cm => cm.Id);

        builder.HasIndex(cm => new { cm.ChannelId, cm.EmployeeId }).IsUnique();

        builder.HasOne(cm => cm.Channel)
            .WithMany(c => c.Members)
            .HasForeignKey(cm => cm.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cm => cm.Employee)
            .WithMany(e => e.ChannelMemberships)
            .HasForeignKey(cm => cm.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
