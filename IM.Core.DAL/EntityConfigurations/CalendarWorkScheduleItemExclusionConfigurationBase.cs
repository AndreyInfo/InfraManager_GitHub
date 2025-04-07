using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class CalendarWorkScheduleItemExclusionConfigurationBase : IEntityTypeConfiguration<CalendarWorkScheduleItemExclusion>
{
    protected abstract string KeyName { get; }
    protected abstract string DFID { get; }
    protected abstract string ExclusionFK { get; }

    public void Configure(EntityTypeBuilder<CalendarWorkScheduleItemExclusion> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyName);

        builder.Property(x => x.ID).HasDefaultValueSql(DFID);


        builder.HasOne(x => x.Exclusion)
            .WithMany()
            .HasForeignKey(x => x.ExclusionID)
            .HasConstraintName(ExclusionFK);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<CalendarWorkScheduleItemExclusion> builder);
}
