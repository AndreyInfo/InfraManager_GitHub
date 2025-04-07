using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.Services.ScheduleService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    public class ScheduleTaskConfiguration : ScheduleTaskConfigurationBase
    {
        protected override string PrimaryKeyName => "schedule_task_pkey";

        protected override string CurrentExecutingScheduleForeignKey =>
            "fk_schedule_task_current_executing_schedule_id";

        protected override void ConfigureDatabase(EntityTypeBuilder<ScheduleTaskEntity> builder)
        {
            builder.ToTable("schedule_task", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.TaskType).HasColumnName("task_type");
            builder.Property(x => x.TaskSettingID).HasColumnName("task_setting_id");
            builder.Property(x => x.TaskSettingName).HasColumnName("task_setting_name");
            builder.Property(x => x.Note).HasMaxLength(1000).HasColumnName("note");
            builder.Property(x => x.IsEnabled).HasColumnName("is_enabled");
            builder.Property(x => x.UseAccount).HasColumnName("use_account");
            builder.Property(x => x.NextRunAt).HasColumnName("next_run_at");
            builder.Property(x => x.FinishRunAt).HasColumnName("finish_run_at");
            builder.Property(x => x.TaskState).HasColumnName("task_state");
            builder.Property(x => x.CredentialID).HasColumnName("credential_id");
            builder.Property(x => x.LastStartAt).HasColumnName("last_run_at");
            builder.Property(x => x.CurrentExecutingScheduleID).HasColumnName("current_executing_schedule_id");
        }
    }
}