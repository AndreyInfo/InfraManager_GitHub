using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CallWaitingStampConfiguration : IEntityTypeConfiguration<CallWaitingStamp>
    {
        public void Configure(EntityTypeBuilder<CallWaitingStamp> builder)
        {
            builder.ToTable("CallWaitingStamp", "dbo");

            builder.HasKey(e => new { e.CallID, e.UtcDateWaitingStarted });

            builder.Property(x => x.CallID)
                .IsRequired();

            builder.Property(x => x.UtcDateWaitingStarted)
                .IsRequired();

        }
    }
}
