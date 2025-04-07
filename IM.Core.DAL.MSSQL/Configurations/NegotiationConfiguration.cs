using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class NegotiationConfiguration : NegotiationConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Negotiation";

        protected override void ConfigureDatabase(EntityTypeBuilder<Negotiation> builder)
        {
            builder.ToTable("Negotiation", "dbo");

            builder.Property(x => x.IMObjID).HasColumnName("ID");
            builder.Property(x => x.Mode).HasColumnName("Mode");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
            builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
            builder.Property(x => x.RowVersion).IsRowVersion().HasColumnName("RowVersion");
            builder.Property(x => x.Status).HasColumnName("Status");
            builder.Property(x => x.UtcDateVoteEnd).HasColumnName("UtcDateVoteEnd").HasColumnType("datetime");
            builder.Property(x => x.UtcDateVoteStart).HasColumnName("UtcDateVoteStart").HasColumnType("datetime");
        }
    }
}
