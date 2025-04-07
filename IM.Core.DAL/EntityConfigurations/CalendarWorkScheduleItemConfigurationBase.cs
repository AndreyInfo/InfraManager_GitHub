using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class CalendarWorkScheduleItemConfigurationBase : IEntityTypeConfiguration<CalendarWorkScheduleItem>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string CalendarWorkScheduleFK { get; }
    public void Configure(EntityTypeBuilder<CalendarWorkScheduleItem> builder)
    {
        builder.HasKey(x => new { x.CalendarWorkScheduleID, x.DayOfYear }).HasName(PrimaryKeyName);

        builder.HasOne(x => x.CalendarWorkSchedule)
            .WithMany(x => x.WorkScheduleItems)
            .HasForeignKey(x => x.CalendarWorkScheduleID)
            .HasConstraintName(CalendarWorkScheduleFK);

        //TODO добавить FK
        builder.HasMany(p => p.WorkScheduleItemExclusions)
            .WithOne()
            .HasForeignKey(f => new { f.CalendarWorkScheduleID, f.DayOfYear });

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleItem> builder);
}
