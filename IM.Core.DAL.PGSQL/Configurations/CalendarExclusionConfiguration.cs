using IM.Core.DAL.Postgres;
using InfraManager.DAL.Calendar;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    public class CalendarExclusionConfiguration : IEntityTypeConfiguration<CalendarExclusion>
    {
        public void Configure(EntityTypeBuilder<CalendarExclusion> builder)
        {
            builder.ToTable("calendar_exclusion", Options.Scheme);
            builder.HasKey(x => x.ID);

            builder.Property(c => c.ID).HasColumnName("id");
            builder.Property(c => c.ObjectClassID).HasColumnName("object_class_id");
            builder.Property(c => c.ObjectID).HasColumnName("object_id");
            builder.Property(c => c.ExclusionID).HasColumnName("exclusion_id");
            builder.Property(c => c.UtcPeriodStart).HasColumnName("utc_period_start");
            builder.Property(c => c.UtcPeriodEnd).HasColumnName("utc_period_end");
            builder.Property(c => c.IsWorkPeriod).HasColumnName("is_work_period");
            builder.Property(c => c.ServiceReferenceID).HasColumnName("service_reference_id");

            builder.HasOne(c => c.Exclusion)
                .WithMany(c => c.CalendarExclusions)
                .HasForeignKey(c => c.ExclusionID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.ServiceReference)
                .WithMany()
                .HasForeignKey(c => c.ServiceReferenceID)
                .HasConstraintName("fk_service_reference_service")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}