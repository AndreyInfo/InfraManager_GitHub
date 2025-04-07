using InfraManager.Services.ScheduleService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ScheduleConfigurationBase : IEntityTypeConfiguration<ScheduleEntity>
    {
        #region Keys

        protected abstract string ScheduleForeignKey { get; }

        protected abstract string PrimaryKeyName { get; }

        #endregion
        #region configuration
        public void Configure(EntityTypeBuilder<ScheduleEntity> builder)
        {
            builder.HasKey(e => e.ID).HasName(PrimaryKeyName);
            builder.Property(x => x.StartAt).HasMaxLength(250);
            builder.Property(x => x.ScheduleType).IsRequired(true);
            builder.Property(x => x.DaysOfWeek).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.Months).HasMaxLength(100);
            builder.Property(x => x.ScheduleTaskEntityID).IsRequired(true);
            


            builder.HasOne(d => d.ScheduleTask)
                 .WithMany(p=>p.Schedules)
            .HasForeignKey(c => c.ScheduleTaskEntityID)
            .HasConstraintName(ScheduleForeignKey)
            .OnDelete(DeleteBehavior.Cascade);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ScheduleEntity> builder);
        #endregion

       
    }
}
