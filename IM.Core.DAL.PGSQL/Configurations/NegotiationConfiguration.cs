using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class NegotiationConfiguration : NegotiationConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_negotiation";

        protected override void ConfigureDatabase(EntityTypeBuilder<Negotiation> builder)
        {
            builder.ToTable("negotiation", Options.Scheme);

            builder.Property(x => x.IMObjID).HasColumnName("id");
            builder.Property(x => x.Mode).HasColumnName("mode");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.ObjectClassID).HasColumnName("object_class_id");
            builder.Property(x => x.ObjectID).HasColumnName("object_id");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(x => x.Status).HasColumnName("status");
            builder.Property(x => x.UtcDateVoteEnd).HasColumnName("utc_date_vote_end");
            builder.Property(x => x.UtcDateVoteStart).HasColumnName("utc_date_vote_start");
        }
    }
}