using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class CalendarWorkScheduleConfiguration : LookupConfiguration<CalendarWorkSchedules.CalendarWorkSchedule>
    {
        protected override string TableName => "calendar_work_schedule";

        protected override void ConfigureAdditionalMembers(
            EntityTypeBuilder<CalendarWorkSchedules.CalendarWorkSchedule> builder)
        {

            builder.HasKey(e => e.ID).HasName("pk_calendar_work_schedule");

            builder.Property(e => e.ID).HasDefaultValueSql("(gen_random_uuid())").HasColumnName("id");
            builder.HasIndex(c=> c.Name, "ui_calendar_work_schedule_name").IsUnique();
            builder.Property(e => e.Name).HasColumnName("name");
            builder.HasXminRowVersion(e => e.RowVersion);

            builder.Property(x => x.Year)
                .IsRequired()
                .HasColumnName("year");

            builder.Property(x => x.ShiftTemplate)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("shift_template");

            builder.Property(x => x.ShiftTemplateLeft)
                .IsRequired()
                .HasColumnType("smallint")
                .HasColumnName("shift_template_left");

            builder.Property(x => x.Note)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("note");

            builder.HasMany(c => c.WorkScheduleItems)
                .WithOne(c => c.CalendarWorkSchedule)
                .HasForeignKey(c => c.CalendarWorkScheduleID)
                .HasConstraintName("fk_calendar_work_schedule_item_calendar_work_schedule");
        }
    }
}