using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class OperationalLevelAgreementConfiguration : OperationalLevelAgreementConfigurationBase
{
    protected override string PrimaryKeyName => "PK_OperationLevelAgreement";
    protected override string UniqueIndexName => "UI_OperationLevelAgreement_Name";
    protected override string FormForeignKey => "FK_OperationLevelAgreement_form";
    
    protected override void ConfigureDataBase(EntityTypeBuilder<OperationalLevelAgreement> builder)
    {
        builder.ToTable("OperationLevelAgreement", Options.Scheme);
        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.Number).HasColumnName("Number");
        builder.Property(x => x.RowVersion).HasColumnName("RowVersion").IsRowVersion();
        builder.Property(x => x.TimeZoneID).HasColumnName("TimeZoneID");
        builder.Property(x => x.CalendarWorkScheduleID).HasColumnName("CalendarWorkScheduleID");
        builder.Property(x => x.IMObjID).HasColumnName("IMObjID");
        builder.Property(x => x.UtcFinishDate).HasColumnName("UtcFinishDate");
        builder.Property(x => x.UtcStartDate).HasColumnName("UtcStartDate");
    }
}