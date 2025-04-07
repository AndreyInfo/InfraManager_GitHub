using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class RoomTypeConfiguration : RoomTypeConfigurationBase
{
    protected override string KeyName => "PK_Типы комнат";

    protected override void ConfigureDatabase(EntityTypeBuilder<RoomType> builder)
    {
        builder.ToTable("Типы комнат", "dbo");

        builder.Property(c => c.ID).HasColumnName("Идентификатор");
        builder.Property(c => c.Name).HasColumnName("Название");
        builder.Property(c => c.ComplementaryID).HasColumnName("ComplementaryID");
    }
}