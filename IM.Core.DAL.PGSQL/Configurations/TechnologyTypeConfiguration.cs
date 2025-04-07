using InfraManager.DAL.Configuration;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class TechnologyTypeConfiguration : TechnologyTypeConfigurationBase
{
    protected override string PrimaryKeyName => "pk_technology_kinds";

    protected override string DefaultValueID => "nextval('pk_technology_type_id_seq'::regclass)";

    protected override string UIName => "ui_name_technology_kinds";

    protected override void ConfigureDataBase(EntityTypeBuilder<TechnologyType> entity)
    {
        entity.ToTable("technology_kinds", Options.Scheme);

        entity.Property(e => e.ID).HasColumnName("identificator");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
    }
}