using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class SoundcardConfiguration : SoundcardConfigurationBase
{
    protected override string PrimaryKeyName => "PK_SoundCard";

    protected override void ConfigureDatabase(EntityTypeBuilder<Soundcard> builder)
    {
        builder.ToTable("SoundCard", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID").HasDefaultValueSql("newid()");

        builder.Property(x => x.DMa).HasColumnName("DMA");
        builder.Property(x => x.IRq).HasColumnName("IRQ");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
    }
}
