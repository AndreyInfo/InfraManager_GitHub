using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class CallWaitingStampConfiguration : IEntityTypeConfiguration<CallWaitingStamp>
    {
        public void Configure(EntityTypeBuilder<CallWaitingStamp> builder)
        {
            builder.ToTable("call_waiting_stamp", Options.Scheme);

            builder.HasKey(e => new {e.CallID, e.UtcDateWaitingStarted});

            builder.Property(x => x.CallID)
                .HasColumnName("call_id")
                .IsRequired();

            builder.Property(x => x.UtcDateWaitingStarted)
                .IsRequired()
                .HasColumnName("utc_date_waiting_started");

            builder.Property(x => x.UtcDateWaitingFinished)
                .HasColumnName("utc_date_waiting_finished");
        }
    }
}