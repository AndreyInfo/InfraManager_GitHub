using InfraManager.DAL.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.EntityConfigurations;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class TechnologyTypeCompatibilityConfiguration : TechnologyTypeCompatibilityConfigurationBase
{
    protected override string PrimaryKeyName => "PK_TechnologyCompatibility";

    protected override string FromForeignKey => "FK_From_TechnologyCompatibility";

    protected override string ToForeignKey => "FK_To_TechnologyCompatibility";

    protected override void ConfigureDataBase(EntityTypeBuilder<TechnologyCompatibilityNode> entity)
    {
        entity.ToTable("TechnologyCompatibility", Options.Scheme);

        entity.Property(c => c.TechIDFrom).HasColumnName("TechID1");
        entity.Property(c => c.TechIDTo).HasColumnName("TechID2");
    }
}
