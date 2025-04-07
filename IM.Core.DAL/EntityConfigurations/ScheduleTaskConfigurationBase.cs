using InfraManager.DAL.MaintenanceWork;
using InfraManager.Services.ScheduleService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ScheduleTaskConfigurationBase : IEntityTypeConfiguration<ScheduleTaskEntity>
    {
        #region configuration
        public void Configure(EntityTypeBuilder<ScheduleTaskEntity> builder)
        {
            builder.HasKey(e => e.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.Name).HasMaxLength(250);
            builder.Property(x => x.TaskType);
            builder.Property(x => x.TaskSettingID);
            builder.Property(x => x.TaskSettingName);
            builder.Property(x => x.Note).HasMaxLength(1000);
            builder.Property(x => x.IsEnabled).IsRequired(true);
            builder.Property(x => x.UseAccount).IsRequired(true);
            builder.Property(x => x.NextRunAt);
            builder.Property(x => x.FinishRunAt);
            builder.Property(x => x.CredentialID);
            builder.Property(x => x.LastStartAt);

            builder.HasOne(x => x.CurrentSchedule).WithMany().HasForeignKey(x => x.CurrentExecutingScheduleID)
                .HasConstraintName(CurrentExecutingScheduleForeignKey).IsRequired(false);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ScheduleTaskEntity> builder);
        #endregion

        #region Keys

        protected abstract string PrimaryKeyName { get; }
        protected abstract string CurrentExecutingScheduleForeignKey { get; }
        
        #endregion
    }
}
