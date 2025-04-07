using InfraManager.DAL;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    public class ScheduleTaskConfiguration : ScheduleTaskConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ScheduleTask";
        protected override string CurrentExecutingScheduleForeignKey => "FK_Schedule_CurrentExecutingScheduleID";

        protected override void ConfigureDatabase(EntityTypeBuilder<ScheduleTaskEntity> builder)
        {
            builder.ToTable("ScheduleTask", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.TaskType).HasColumnName("TaskType");
            builder.Property(x => x.TaskSettingID).HasColumnName("TaskSettingID");
            builder.Property(x => x.TaskSettingName).HasColumnName("TaskSettingName");
            builder.Property(x => x.Note).HasMaxLength(1000).HasColumnName("Note");
            builder.Property(x => x.IsEnabled).HasColumnName("IsEnabled");
            builder.Property(x => x.UseAccount).HasColumnName("UseAccount");
            builder.Property(x => x.NextRunAt).HasColumnName("NextRunAt");
            builder.Property(x => x.FinishRunAt).HasColumnName("FinishRunAt");
            builder.Property(x => x.TaskState).HasColumnName("TaskState");
            builder.Property(x => x.CredentialID).HasColumnName("CredentialID");
            builder.Property(x => x.LastStartAt).HasColumnName("LastStartAt");
            builder.Property(x => x.CurrentExecutingScheduleID).HasColumnName("CurrentExecutingScheduleID");
        }
    }
}