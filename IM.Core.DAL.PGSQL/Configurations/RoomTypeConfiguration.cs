using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class RoomTypeConfiguration : RoomTypeConfigurationBase
{
    protected override string KeyName => "pk_room_types";

    protected override void ConfigureDatabase(EntityTypeBuilder<RoomType> builder)
    {
        builder.ToTable("room_types", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("identificator");
        builder.Property(c => c.Name).HasColumnName("name");
        builder.Property(c => c.ComplementaryID).HasColumnName("complementary_id");
    }
}