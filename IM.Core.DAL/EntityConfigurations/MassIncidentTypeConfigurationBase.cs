using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class MassIncidentTypeConfigurationBase : IEntityTypeConfiguration<MassIncidentType>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string NameUniqueKeyName { get; }
        protected abstract string DefaultValueIMObjID { get; }
        protected abstract string FormForeignKeyName { get; }

        public void Configure(EntityTypeBuilder<MassIncidentType> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.HasIndex(x => x.Name, NameUniqueKeyName).IsUnique();   
            
            builder.Property(x => x.ID).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(255);
            builder.Property(x => x.WorkflowSchemeIdentifier).IsRequired(false).HasMaxLength(255);
            builder.Property(x => x.IMObjID).ValueGeneratedOnAdd().HasDefaultValueSql(DefaultValueIMObjID);

            builder.IsMarkableForDelete();

            builder.HasOne(x => x.Form)
                .WithMany()
                .HasForeignKey(x => x.FormID)
                .HasPrincipalKey(x => x.ID)
                .IsRequired(false)
                .HasConstraintName(FormForeignKeyName);

            ConfigureDataProvider(builder);
        }

        protected abstract void ConfigureDataProvider(EntityTypeBuilder<MassIncidentType> builder);
    }
}
