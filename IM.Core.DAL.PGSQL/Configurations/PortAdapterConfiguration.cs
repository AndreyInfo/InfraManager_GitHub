using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class PortAdapterConfiguration : PortAdapterConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_port_adapter";

        protected override string Schema => Options.Scheme;

        protected override string TableName => "port_adapter";

        protected override void AdditionalConfig(EntityTypeBuilder<PortAdapter> entity)
        {
            entity.Property(e => e.ID).HasColumnName("id");
            entity.Property(e => e.ObjectID).HasColumnName("object_id");
            entity.Property(e => e.PortNumber).HasColumnName("number");
            entity.Property(e => e.JackTypeID).HasColumnName("jack_type_id");
            entity.Property(e => e.TechnologyID).HasColumnName("technology_type_id");
            entity.Property(e => e.PortAddress).HasColumnName("port_address");
            entity.Property(e => e.Note).HasColumnName("note");

            entity.HasOne(x => x.JackType)
                .WithMany()
                .HasForeignKey(x => x.JackTypeID)
                .HasConstraintName("fk_port_adapter_jack_type");

            entity.HasOne(x => x.TechnologyType)
                .WithMany()
                .HasForeignKey(x => x.TechnologyID)
                .HasConstraintName("fk_port_adapter_technology_type");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PortAdapter> entity);
    }
}
