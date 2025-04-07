using InfraManager.DAL.Calendar;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ExclusionConfigurationBase : IEntityTypeConfiguration<Exclusion>
{
    protected abstract string KeyName { get; }
    protected abstract string FKCalendarExclusionsName { get; }
    protected abstract string DefaultValueSQL { get; }
    protected abstract string UIName { get; }

    public void Configure(EntityTypeBuilder<Exclusion> builder)
    {
        builder.HasKey(c => c.ID).HasName(KeyName);

        builder.HasIndex(c => c.Name, UIName).IsUnique();

        builder.Property(c => c.ID).HasDefaultValueSql(DefaultValueSQL).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(250);

        builder.HasMany(x => x.CalendarExclusions)
            .WithOne(x => x.Exclusion)
            .HasForeignKey(x => x.ExclusionID)
            .HasConstraintName(FKCalendarExclusionsName)
            .OnDelete(DeleteBehavior.Cascade);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<Exclusion> builder);
}
