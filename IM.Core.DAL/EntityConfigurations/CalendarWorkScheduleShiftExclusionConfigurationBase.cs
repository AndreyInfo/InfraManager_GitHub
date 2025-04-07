using InfraManager.DAL.CalendarWorkSchedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class CalendarWorkScheduleShiftExclusionConfigurationBase : IEntityTypeConfiguration<CalendarWorkScheduleShiftExclusion>
{
    protected abstract string KeyName { get; }
    protected abstract string DefaultValueID { get; }
    protected abstract string  CalendarWorkScheduleShiftFK {get;}
    protected abstract string  ExclusionFK {get;}

    public void Configure(EntityTypeBuilder<CalendarWorkScheduleShiftExclusion> builder)
    {
        builder.HasKey(c => c.ID);

        builder.Property(e => e.ID).HasDefaultValueSql(DefaultValueID);

        builder.HasOne(p => p.CalendarWorkScheduleShift)
            .WithMany(p => p.WorkScheduleShiftExclusions)
            .HasForeignKey(p => p.CalendarWorkScheduleShiftID)
            .HasConstraintName(CalendarWorkScheduleShiftFK);

        builder.HasOne(p => p.Exclusion)
            .WithMany()
            .HasForeignKey(p => p.ExclusionID)
            .HasConstraintName(ExclusionFK);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<CalendarWorkScheduleShiftExclusion> builder);
}
