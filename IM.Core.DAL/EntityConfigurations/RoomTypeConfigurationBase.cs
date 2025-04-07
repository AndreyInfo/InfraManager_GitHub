using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class RoomTypeConfigurationBase : IEntityTypeConfiguration<RoomType>
{
    protected abstract string KeyName { get; }
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.HasKey(c => c.ID)
            .HasName(KeyName);

        builder.Property(c => c.Name)
            .HasMaxLength(255);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<RoomType> builder);

}
