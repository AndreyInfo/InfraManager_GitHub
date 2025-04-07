using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class MassIncidentCauseConfigurationBase : IEntityTypeConfiguration<MassIncidentCause>
    {
        public void Configure(EntityTypeBuilder<MassIncidentCause> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.ID).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(500).IsRequired(true);
            builder.HasIndex(x => x.IMObjID).HasDatabaseName(IMObjIDUniqueKeyName);
            builder.Property(x => x.IMObjID).ValueGeneratedOnAdd();
            builder.IsMarkableForDelete();

            ConfigureDataProvider(builder);
        }

        protected abstract void ConfigureDataProvider(EntityTypeBuilder<MassIncidentCause> builder);

        protected abstract string PrimaryKeyName { get; }
        protected abstract string IMObjIDUniqueKeyName { get; }
    }
}
