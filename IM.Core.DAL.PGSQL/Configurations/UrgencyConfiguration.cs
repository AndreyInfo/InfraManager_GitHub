using IM.Core.DAL.PGSQL.Configurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UrgencyConfiguration : LookupConfiguration<Urgency>
    {
        protected override string TableName => "urgency";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<Urgency> builder)
        {
            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(e => e.Sequence).HasColumnName("sequence");

            builder.HasKey(e => e.ID).HasName("pk_urgency");
        }
    }
}