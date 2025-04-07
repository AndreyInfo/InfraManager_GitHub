using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class SoundcardConfiguration : SoundcardConfigurationBase
{
    protected override string PrimaryKeyName => "pk_sound_card";

    protected override void ConfigureDatabase(EntityTypeBuilder<Soundcard> builder)
    {
        builder.ToTable("sound_card", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.DMa).HasColumnName("d_ma");
        builder.Property(x => x.IRq).HasColumnName("i_rq");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
    }
}
