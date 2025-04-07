using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class OperationalLevelAgreementConfiguration : OperationalLevelAgreementConfigurationBase
{
    protected override string PrimaryKeyName => "pk_operational_level_agreement";
    protected override string UniqueIndexName => "ui_operational_level_agreement_name";
    protected override string FormForeignKey => "fk_operational_level_agreement_form";

    protected override void ConfigureDataBase(EntityTypeBuilder<OperationalLevelAgreement> builder)
    {
        builder.ToTable("operational_level_agreement", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.Number).HasColumnName("number");
        builder.Property(x => x.UtcFinishDate).HasColumnName("utc_finish_data");
        builder.Property(x => x.UtcStartDate).HasColumnName("utc_start_data");
        builder.Property(x => x.TimeZoneID).HasColumnName("time_zone_id");
        builder.Property(x => x.CalendarWorkScheduleID).HasColumnName("calendar_work_schedule_id");
        builder.Property(x => x.IMObjID).HasColumnName("im_obj_id");
        builder.Property(x => x.FormID).HasColumnName("form_id");
        builder.HasXminRowVersion(x => x.RowVersion);

        builder.HasOne(x => x.TimeZone).WithMany().HasForeignKey(x => x.TimeZoneID);
        builder.HasOne(x => x.CalendarWorkSchedule).WithMany().HasForeignKey(x => x.CalendarWorkScheduleID);
        builder.HasMany(x => x.ConcludedWith).WithOne().HasForeignKey(x => x.ID).HasPrincipalKey(x => x.IMObjID);
    }
}