using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class CalendarHolidayConfigurationBase : IEntityTypeConfiguration<CalendarHoliday>
{
    protected abstract string KeyName { get; }
    protected abstract string UIName { get; }
    protected abstract string ForeignKeyCalendarHoliday { get; }
    public void Configure(EntityTypeBuilder<CalendarHoliday> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyName);

        builder.HasIndex(c => c.Name, UIName).IsUnique();

        builder.Property(x => x.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasMaxLength(500).IsRequired(true);

        builder.HasMany(c => c.CalendarHolidayItems)
            .WithOne()
            .HasForeignKey(c => c.CalendarHolidayID)
            .HasConstraintName(ForeignKeyCalendarHoliday)
            .OnDelete(DeleteBehavior.Cascade);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<CalendarHoliday> builder);
}
