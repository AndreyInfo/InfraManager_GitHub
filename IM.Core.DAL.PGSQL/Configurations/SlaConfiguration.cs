using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class SlaConfiguration : ServiceLevelAgreementConfigurationBase
    {
        protected override string KeyPK => "pk_sla";
        protected override string UIName => "ui_sla_name";
        protected override string FormForeignKey => "fk_sla_form";

        protected override void ConfigureDataProvider(EntityTypeBuilder<ServiceLevelAgreement> builder)
        {
            builder.ToTable("sla", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(e => e.CalendarWorkScheduleID).HasColumnName("calendar_work_schedule_id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Note).HasColumnName("note");
            builder.Property(e => e.Number).HasColumnName("number");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(e => e.TimeZoneID).HasColumnName("time_zone_id");
            builder.Property(e => e.UtcFinishDate).HasColumnName("utc_finish_date");
            builder.Property(e => e.UtcStartDate).HasColumnName("utc_start_date");
            builder.Property(x => x.FormID).HasColumnName("form_id");
        }
    }
}