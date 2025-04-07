using InfraManager.DAL.Configuration;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class TechnologyTypeConfiguration : TechnologyTypeConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Виды технологий";

    protected override string DefaultValueID => "(NEXT VALUE FOR [PK_TechnologyType_ID_Seq])";

    protected override string UIName => "UI_Name_TechnologyTypes";

    protected override void ConfigureDataBase(EntityTypeBuilder<TechnologyType> entity)
    {
        entity.ToTable("Виды технологий", Options.Scheme);

        entity.Property(f => f.ID).HasColumnName("Идентификатор");
        entity.Property(e => e.Name).HasColumnName("Название");
        entity.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
    }
}
