using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IM.Core.DAL.Postgres;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class CallSummaryConfiguration : IEntityTypeConfiguration<CallSummary>
    {
        public void Configure(EntityTypeBuilder<CallSummary> builder)
        {
            builder.ToTable("call_summary", Options.Scheme);

            builder.HasKey(e => e.ID).HasName("pk_call_summary");

            builder.Property(x => x.ID)
                .HasColumnName("id");
            builder.Property(x => x.Name)
                .HasColumnName("name");
            builder.Property(x => x.ServiceItemID)
                .HasColumnName("service_item_id");
            builder.Property(x => x.ServiceAttendanceID)
                .HasColumnName("service_attendance_id");
            builder.Property(x => x.Visible)
                .HasColumnName("visible");
            builder.HasXminRowVersion(x => x.RowVersion);

            builder.HasOne(x => x.ServiceAttendance)
                .WithMany(p => p.CallSummary)
                .HasForeignKey(x => x.ServiceAttendanceID)
                .HasConstraintName("fk_call_summary_service_attendance");
            builder.HasOne(x => x.ServiceItem)
                .WithMany(p => p.CallSummary)
                .HasForeignKey(x => x.ServiceItemID)
                .HasConstraintName("fk_call_summary_service_item");
        }
    }
}