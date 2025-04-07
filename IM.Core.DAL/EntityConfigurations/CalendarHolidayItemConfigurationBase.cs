using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class CalendarHolidayItemConfigurationBase : IEntityTypeConfiguration<CalendarHolidayItem>
{
    protected abstract string KeyName { get; }

    public void Configure(EntityTypeBuilder<CalendarHolidayItem> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyName);

        builder.Property(x => x.ID).ValueGeneratedOnAdd();

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<CalendarHolidayItem> builder);
}
