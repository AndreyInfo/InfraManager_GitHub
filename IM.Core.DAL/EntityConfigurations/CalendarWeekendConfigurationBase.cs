using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class CalendarWeekendConfigurationBase : IEntityTypeConfiguration<CalendarWeekend>
{
    protected abstract string KeyName { get; }

    protected abstract string UniqueIndexByName { get; }
    public void Configure(EntityTypeBuilder<CalendarWeekend> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyName);

        builder.HasIndex(c => c.Name, UniqueIndexByName).IsUnique();

        builder.Property(x => x.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).HasMaxLength(500).IsRequired(true);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<CalendarWeekend> builder);
}
