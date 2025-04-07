using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CallSummaryConfiguration : IEntityTypeConfiguration<CallSummary>
    {
        public void Configure(EntityTypeBuilder<CallSummary> builder)
        {
            builder.ToTable("CallSummary", "dbo");

            builder.HasKey(e => e.ID).HasName("PK_CallSummary");

            builder.Property(x => x.ID)
                .HasColumnName("ID");
            builder.Property(x => x.Name)
                .HasColumnName("Name");
            builder.Property(x => x.ServiceItemID)
               .HasColumnName("ServiceItemID");
            builder.Property(x => x.ServiceAttendanceID)
               .HasColumnName("ServiceAttendanceID");
            builder.Property(x => x.Visible)
              .HasColumnName("Visible");
            builder.Property(x => x.RowVersion)
              .IsRowVersion();

            builder.HasOne(x => x.ServiceAttendance)
                .WithMany(p => p.CallSummary)
                .HasForeignKey(x => x.ServiceAttendanceID)
                .HasConstraintName("FK_CallSummary_ServiceAttendance");
            builder.HasOne(x => x.ServiceItem)
                .WithMany(p => p.CallSummary)
                .HasForeignKey(x => x.ServiceItemID)
                .HasConstraintName("FK_CallSummary_ServiceItem");
        }
    }
}
