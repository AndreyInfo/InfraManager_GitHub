using InfraManager.DAL.EntityConfigurations;
using InfraManager.Services.ScheduleService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    public class ScheduleConfiguration : ScheduleConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Schedule";
        protected override string ScheduleForeignKey => "FK_Schedule_ScheduleTask";

        protected override void ConfigureDatabase(EntityTypeBuilder<ScheduleEntity> builder)
        {
            builder.ToTable("Schedule", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.StartAt).HasColumnName("StartAt");
            builder.Property(x => x.Interval).HasColumnName("Interval");
            builder.Property(x => x.FinishAt).HasColumnName("FinishAt");
            builder.Property(x => x.ScheduleType).HasColumnName("ScheduleType");
            builder.Property(x => x.DaysOfWeek).HasColumnName("DaysOfWeek");
            builder.Property(x => x.Months).HasColumnName("Months");
            builder.Property(x => x.ScheduleTaskEntityID).HasColumnName("ScheduleTaskEntityID");
            builder.Property(x => x.NextAt).HasColumnType("datetime").HasColumnName("NextAt");
            builder.Property(x => x.LastAt).HasColumnType("datetime").HasColumnName("LastAt");
        }
    }
}