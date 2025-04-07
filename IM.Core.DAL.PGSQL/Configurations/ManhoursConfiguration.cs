using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.Manhours;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ManhoursConfiguration : ManhoursConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_manhours";

        protected override void ConfigureDatabase(EntityTypeBuilder<ManhoursEntry> builder)
        {
            builder.ToTable("manhours", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.WorkID).HasColumnName("manhours_work_id");
            builder.Property(x => x.UtcDate).HasColumnName("utc_date");
            builder.Property(x => x.Value).HasColumnName("value_in_minutes");
        }
    }
}