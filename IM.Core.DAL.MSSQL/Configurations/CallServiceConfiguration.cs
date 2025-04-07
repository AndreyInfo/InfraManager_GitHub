using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CallServiceConfiguration : IEntityTypeConfiguration<CallService>
    {
        public void Configure(EntityTypeBuilder<CallService> builder)
        {
            builder.ToTable("CallService", "dbo");

            builder.HasKey(e => e.ID);
            builder.Property(x => x.ServiceID).HasColumnName("ServiceID");
            builder.Property(x => x.ServiceItemID).HasColumnName("ServiceItemID");
            builder.Property(x => x.ServiceAttendanceID).HasColumnName("ServiceAttendanceID");

            builder.Property(x => x.ID)
                .HasColumnName("ID")
                .HasDefaultValueSql("(newid())");

            builder.Property(x => x.ServiceName)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("ServiceName");

            builder.Property(x => x.ServiceItemOrAttendanceName)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("ServiceItemOrAttendanceName");

            builder.HasOne(x => x.Service)
                .WithMany()
                .HasForeignKey(x => x.ServiceID)
                .HasConstraintName("FK_CallService_Service");

            builder.HasOne(x => x.ServiceItem)
                .WithMany()
                .HasForeignKey(x => x.ServiceItemID)
                .HasConstraintName("FK_CallService_ServiceItem");

            builder.HasOne(x => x.ServiceAttendance)
                .WithMany()
                .HasForeignKey(x => x.ServiceAttendanceID)
                .HasConstraintName("FK_CallService_ServiceAttendance");
        }
    }
}
