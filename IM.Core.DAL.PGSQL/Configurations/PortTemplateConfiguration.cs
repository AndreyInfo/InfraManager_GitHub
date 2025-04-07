using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class PortTemplateConfiguration : PortTemplateConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_port_template";

        protected override string Schema => Options.Scheme;
        
        protected override string TableName => "port_template";

        protected override void AdditionalConfig(EntityTypeBuilder<PortTemplate> entity)
        {
            entity.Property(e => e.ObjectID).HasColumnName("object_id");
            entity.Property(e => e.ClassID).HasColumnName("object_class_id");
            entity.Property(e => e.PortNumber).HasColumnName("number");
            entity.Property(e => e.JackTypeID).HasColumnName("jack_type_id");
            entity.Property(e => e.TechnologyID).HasColumnName("technology_type_id");

            entity.HasIndex(e => e.ObjectID).HasDatabaseName("ix_port_template_object_id");
            
            entity.HasOne(x => x.JackType)
                .WithMany()
                .HasForeignKey(x => x.JackTypeID)
                .HasConstraintName("fk_port_template_jack_type");

            entity.HasOne(x => x.TechnologyType)
                .WithMany()
                .HasForeignKey(x => x.TechnologyID)
                .HasConstraintName("fk_port_template_technology_type");
            
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PortTemplate> entity);
    }
}
