using InfraManager.DAL.Calendar;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class CalendarExclusionConfiguration : IEntityTypeConfiguration<CalendarExclusion>
    {
        public void Configure(EntityTypeBuilder<CalendarExclusion> builder)
        {
            builder.ToTable("CalendarExclusion", "dbo");
            builder.HasKey(x => x.ID);

            builder.HasOne(c => c.Exclusion)
                .WithMany(c => c.CalendarExclusions)
                .HasForeignKey(c => c.ExclusionID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.ServiceReference)
                .WithMany()
                .HasForeignKey(c => c.ServiceReferenceID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
