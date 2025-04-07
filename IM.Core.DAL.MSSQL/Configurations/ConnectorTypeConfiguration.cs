using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ConnectorTypeConfiguration : ConnectorTypeConfigurationBase
{
    protected override string PrimaryKey => "PK_Виды_разъемов";

    protected override string MediumForeignKey => "FK_ConnectorType_Medium";

    protected override string UIName => "UI_Name_ConnectorTypes";

    protected override void ConfigureDatabase(EntityTypeBuilder<ConnectorType> builder)
    {
        builder.ToTable("Виды разъемов", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("Идентификатор");
        builder.Property(x => x.Name).HasColumnName("Название");
        builder.Property(x => x.PairCount).HasColumnName("Количество пар");
        builder.Property(x => x.MediumID).HasColumnName("MediumID");
        builder.Property(x => x.Image).HasColumnName("Изображение");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
    }
}
