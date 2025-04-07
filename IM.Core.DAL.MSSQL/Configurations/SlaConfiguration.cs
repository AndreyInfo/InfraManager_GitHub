using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class SlaConfiguration : ServiceLevelAgreementConfigurationBase
    {
        protected override string KeyPK => "PK_SLA";
        protected override string UIName => "UI_SLA_Name";
        protected override string FormForeignKey => "FK_SLA_Form";

        protected override void ConfigureDataProvider(EntityTypeBuilder<ServiceLevelAgreement> builder)
        {
            builder.ToTable("SLA", "dbo");
            
            builder.Property(x => x.Name).HasColumnName("Name");

            builder.Property(x => x.FormID).HasColumnName("FormID");

            builder.Property(x => x.Note).HasColumnName("Note");

            builder.Property(x => x.Number).HasColumnName("Number");

            builder.Property(x => x.TimeZoneID).HasColumnName("TimeZoneID");

            builder.Property(x => x.RowVersion).IsRowVersion().HasColumnName("RowVersion");

            builder.Property(x => x.CalendarWorkScheduleID).HasColumnName("CalendarWorkScheduleID");

            builder.Property(x => x.UtcStartDate).HasColumnType("datetime").HasColumnName("UtcStartDate");

            builder.Property(x => x.UtcFinishDate).HasColumnType("datetime").HasColumnName("UtcFinishDate");
        }
    }
}
