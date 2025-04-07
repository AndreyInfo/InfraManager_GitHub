using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class InfluenceConfiguration : LookupConfiguration<Influence>
    {
        protected override string TableName => "influence";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<Influence> builder)
        {
            builder.ToTable("influence");
            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(e => e.Sequence).HasColumnName("sequence");
            // Influence => influence
            builder.HasKey(x => x.ID).HasName("pk_influence");
        }
    }
}