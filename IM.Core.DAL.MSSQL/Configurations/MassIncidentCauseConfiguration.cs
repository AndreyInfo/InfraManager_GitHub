using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class MassIncidentCauseConfiguration : MassIncidentCauseConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_MassiveIncidentCause";
        protected override string IMObjIDUniqueKeyName => "UK_TechnicalFailuresCategory_IMObjID";

        protected override void ConfigureDataProvider(EntityTypeBuilder<MassIncidentCause> builder)
        {
            builder.ToTable("MassIncidentCause", "dbo");

            builder.Property(x => x.RowVersion).IsRowVersion().HasColumnName("RowVersion");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Removed).HasColumnName("Removed");
            builder.Property(x => x.IMObjID).HasColumnName("IMObjID").HasDefaultValueSql("(newid())");
        }
    }
}
