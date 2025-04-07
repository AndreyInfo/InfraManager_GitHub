using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public partial class PortAdapterConfiguration : PortAdapterConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_AdapterPort";
        protected override string Schema => Options.Scheme;
        protected override string TableName => "AdapterPort";

        protected override void AdditionalConfig(EntityTypeBuilder<PortAdapter> entity)
        {
            entity.Property(e => e.ID).HasColumnName("ID");
            entity.Property(e => e.ObjectID).HasColumnName("ObjectID");
            entity.Property(e => e.PortNumber).HasColumnName("Number");
            entity.Property(e => e.JackTypeID).HasColumnName("JackTypeID");
            entity.Property(e => e.TechnologyID).HasColumnName("TechnologyTypeID");
            entity.Property(e => e.PortAddress).HasColumnName("PortAddress");
            entity.Property(e => e.Note).HasColumnName("Note");

            entity.HasOne(x => x.JackType)
                .WithMany()
                .HasForeignKey(x => x.JackTypeID)
                .HasConstraintName("FK_AdapterPort_JackType");

            entity.HasOne(x => x.TechnologyType)
                .WithMany()
                .HasForeignKey(x => x.TechnologyID)
                .HasConstraintName("FK_AdapterPort_TechnologyType");
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<PortAdapter> entity);
    }
}
