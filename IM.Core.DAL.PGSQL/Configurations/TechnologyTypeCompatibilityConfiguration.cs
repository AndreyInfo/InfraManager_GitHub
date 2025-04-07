using InfraManager.DAL.Configuration;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class TechnologyTypeCompatibilityConfiguration : TechnologyTypeCompatibilityConfigurationBase
{
    protected override string PrimaryKeyName => "technology_compatibility_pkey";

    protected override string FromForeignKey => "fk_from_technology_compatibility";

    protected override string ToForeignKey => "fk_to_technology_compatibility";

    protected override void ConfigureDataBase(EntityTypeBuilder<TechnologyCompatibilityNode> entity)
    {
        entity.ToTable("technology_compatibility", Options.Scheme);

        entity.Property(c => c.TechIDFrom).HasColumnName("tech_id1");
        entity.Property(c => c.TechIDTo).HasColumnName("tech_id2");
    }
}