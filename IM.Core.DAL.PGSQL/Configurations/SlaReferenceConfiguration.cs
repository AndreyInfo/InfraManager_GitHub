using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class SlaReferenceConfiguration : SlaReferenceConfigurationBase
    {
        protected override string UISlaSlaReference => "ui_sla_slaReference";

        protected override void ConfigureDatabase(EntityTypeBuilder<SLAReference> builder)
        {
            builder.ToTable("sla_reference", Options.Scheme);

            builder.Property(x => x.SLAID).HasColumnName("sla_id");
            builder.Property(x => x.ObjectID).HasColumnName("object_id");
            builder.Property(x => x.ClassID).HasColumnName("class_id");
            builder.Property(x => x.TimeZoneID).HasColumnName("time_zone_id");
            builder.Property(x => x.CalendarWorkScheduleID).HasColumnName("calendar_work_schedule_id");
        }
    }
}
