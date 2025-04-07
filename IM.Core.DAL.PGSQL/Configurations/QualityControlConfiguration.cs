using IM.Core.DAL.Postgres;
using Core = InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InfraManager.DAL.ServiceDesk.Quality;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class QualityControlConfiguration : Core.QualityControlConfiguration
    {
        protected override string PrimaryKeyName => "pk_quality_contol";


        protected override void ConfigureDatabase(EntityTypeBuilder<QualityControl> builder)
        {
            builder.ToTable("quality_control", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id").HasColumnType("uuid");
            builder.Property(x => x.Type).HasColumnName("type").HasColumnType("smallint");
            builder.Property(x => x.UtcDate).HasColumnName("utc_date");
            builder.Property(x => x.CallID).HasColumnName("call_id").HasColumnType("uuid");
            builder.Property(x => x.TimeSpanInMinutes).HasColumnName("time_span_in_minutes");
            builder.Property(x => x.TimeSpanInWorkMinutes).HasColumnName("time_span_in_work_minutes");

        }
    }
}
