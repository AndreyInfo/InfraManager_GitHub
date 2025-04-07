using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    internal class SlaReferenceConfiguration : SlaReferenceConfigurationBase
    {
        protected override string UISlaSlaReference => "UI_Sla_SlaReference";

        protected override void ConfigureDatabase(EntityTypeBuilder<SLAReference> builder)
        {
            builder.ToTable("SLAReference", "dbo");

            builder.Property(x => x.SLAID).HasColumnName("SLAID");
            builder.Property(x => x.ObjectID).HasColumnName("ObjectId");
            builder.Property(x => x.ClassID).HasColumnName("ClassId");
            builder.Property(x => x.TimeZoneID).HasColumnName("TimeZoneID");
            builder.Property(x => x.CalendarWorkScheduleID).HasColumnName("CalendarWorkScheduleID");
        }
    }
}
