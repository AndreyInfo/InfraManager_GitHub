using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.Services.ScheduleService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    public class ScheduleConfiguration : ScheduleConfigurationBase
    {
        protected override string PrimaryKeyName => "task_schedule_pkey";
        protected override string ScheduleForeignKey => "task_schedule_schedule_task";

        protected override void ConfigureDatabase(EntityTypeBuilder<ScheduleEntity> builder)
        {
            builder.ToTable("schedule", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.StartAt).HasColumnName("start_at");
            builder.Property(x => x.Interval).HasColumnName("interval");
            builder.Property(x => x.FinishAt).HasColumnName("finish_at");
            builder.Property(x => x.ScheduleType).HasColumnName("schedule_type");
            builder.Property(x => x.DaysOfWeek).HasColumnName("days_of_week");
            builder.Property(x => x.Months).HasColumnName("months");
            builder.Property(x => x.ScheduleTaskEntityID).HasColumnName("schedule_task_id");
            builder.Property(x => x.NextAt).HasColumnType("timestamp(3)").HasColumnName("next_at");
            builder.Property(x => x.LastAt).HasColumnType("timestamp(3)").HasColumnName("last_at");
        }
    }
}