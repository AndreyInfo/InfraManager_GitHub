using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class CallServiceConfiguration : IEntityTypeConfiguration<CallService>
    {
        public void Configure(EntityTypeBuilder<CallService> builder)
        {
            builder.ToTable("call_service", Options.Scheme);

            builder.HasKey(e => e.ID).HasName("pk_call_service");
            builder.Property(x => x.ServiceID).HasColumnName("service_id");
            builder.Property(x => x.ServiceItemID).HasColumnName("service_item_id");
            builder.Property(x => x.ServiceAttendanceID).HasColumnName("service_attendance_id");

            builder.Property(x => x.ID)
                .HasColumnName("id")
                .HasDefaultValueSql("(gen_random_uuid())");

            builder.Property(x => x.ServiceName)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("service_name");

            builder.Property(x => x.ServiceItemOrAttendanceName)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("service_item_or_attendance_name");

            builder.HasOne(x => x.Service)
                .WithMany()
                .HasForeignKey(x => x.ServiceID)
                .HasConstraintName("fk_call_service_service");

            builder.HasOne(x => x.ServiceItem)
                .WithMany()
                .HasForeignKey(x => x.ServiceItemID)
                .HasConstraintName("fk_call_service_service_item");

            builder.HasOne(x => x.ServiceAttendance)
                .WithMany()
                .HasForeignKey(x => x.ServiceAttendanceID)
                .HasConstraintName("fk_call_service_service_attendance");
        }
    }
}