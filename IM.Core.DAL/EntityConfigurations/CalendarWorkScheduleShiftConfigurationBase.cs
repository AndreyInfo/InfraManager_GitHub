using InfraManager.DAL.CalendarWorkSchedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class CalendarWorkScheduleShiftConfigurationBase : IEntityTypeConfiguration<CalendarWorkScheduleShift>
{
    protected abstract string PrimaryKey { get; }
    protected abstract string CalendarWorkcScheduleNumberUI { get; }
    protected abstract string DefaultValueID { get; }
    protected abstract string CalendarWorkScheduleFK { get; }

    public void Configure(EntityTypeBuilder<CalendarWorkScheduleShift> builder)
    {
        builder.HasKey(c => c.ID).HasName(PrimaryKey);

        builder.HasIndex(c => new { c.CalendarWorkScheduleID, c.Number }, CalendarWorkcScheduleNumberUI).IsUnique();

        builder.Property(e => e.ID).HasDefaultValueSql(DefaultValueID).ValueGeneratedOnAdd();

        builder.HasOne(p => p.CalendarWorkSchedule)
            .WithMany(p => p.WorkScheduleShifts)
            .HasForeignKey(p => p.CalendarWorkScheduleID)
            .HasConstraintName(CalendarWorkScheduleFK);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleShift> builder);
}
