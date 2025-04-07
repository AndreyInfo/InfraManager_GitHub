using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CalendarWorkScheduleConfiguration : LookupConfiguration<CalendarWorkSchedules.CalendarWorkSchedule>
    {
        protected override string TableName => "CalendarWorkSchedule";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<CalendarWorkSchedules.CalendarWorkSchedule> builder)
        {
            builder.HasIndex(c=> c.Name, "UI_CalendarWorkSchedule_UniqueName").IsUnique();

            builder.Property(x => x.Year)
                .IsRequired()
                .HasColumnName("Year");

            builder.Property(x => x.ShiftTemplate)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("ShiftTemplate");

            builder.Property(x => x.ShiftTemplateLeft)
                .IsRequired()
                .HasColumnType("tinyint")
                .HasColumnName("ShiftTemplateLeft");

            builder.Property(x => x.Note)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("Note");


            builder.HasMany(c => c.WorkScheduleShifts)
                 .WithOne(c => c.CalendarWorkSchedule)
                 .HasForeignKey(f => f.CalendarWorkScheduleID)
                 .HasConstraintName("FK_CalendarWorkScheduleShift_CalendarWorkSchedule");


            builder.HasMany(c => c.WorkScheduleItems)
                .WithOne(c => c.CalendarWorkSchedule)
                .HasForeignKey(c => c.CalendarWorkScheduleID)
                .HasConstraintName("FK_CalendarWorkScheduleItem_CalendarWorkSchedule");


            builder.Property(p => p.RowVersion).IsRowVersion();

            builder.Property(e => e.Note).HasMaxLength(500);
            builder.Property(e => e.Name).HasMaxLength(250);
            builder.Property(e => e.ShiftTemplate).HasMaxLength(50);
        }
    }
}
