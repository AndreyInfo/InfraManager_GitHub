using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class CalendarWorkScheduleDefaultConfigurationBase : IEntityTypeConfiguration<CalendarWorkScheduleDefault>
{
    protected abstract string CalendarWeekendFK { get; }
    protected abstract string CalendarHolidayFK { get; }
    protected abstract string TimeZoneFK { get; }
    protected abstract string KeyName { get; }

    public void Configure(EntityTypeBuilder<CalendarWorkScheduleDefault> builder)
    {
        builder.HasKey(c=> c.ID).HasName(KeyName);

        builder.Property(x => x.TimeZoneID).HasMaxLength(500).IsRequired(true);

        builder.HasOne(c => c.CalendarWeekend)
            .WithMany()
            .HasForeignKey(c => c.CalendarWeekendID)
            .HasConstraintName(CalendarWeekendFK);

        builder.HasOne(c => c.CalendarHoliday)
            .WithMany()
            .HasForeignKey(c => c.CalendarHolidayID)
            .HasConstraintName(CalendarHolidayFK);

        builder.HasOne(c => c.TimeZone)
            .WithMany()
            .HasForeignKey(c => c.TimeZoneID)
            .HasConstraintName(TimeZoneFK);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleDefault> builder);
}
