using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class SubdivisionConfiguration : SubdivisionConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Подразделение";

        protected override string InxedUniqueName => "UI_Subdivision_organization_ParentSubdivision_Name";

        protected override void ConfigureDataProvider(EntityTypeBuilder<Subdivision> builder)
        {
            builder.ToTable("Подразделение", "dbo");


            builder.Property(e => e.ID).HasColumnName("Идентификатор");
            builder.Property(e => e.OrganizationID).HasColumnName("ИД организации");
            builder.Property(e => e.CalendarWorkScheduleID).HasColumnName("CalendarWorkScheduleID");
            builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
            builder.Property(e => e.ExternalID).HasColumnName("ExternalID").HasDefaultValueSql("('')");
            builder.Property(e => e.IsLockedForOsi).HasColumnName("IsLockedForOSI");
            builder.Property(e => e.It).HasColumnName("IT");
            builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
            builder.Property(e => e.SubdivisionID).HasColumnName("ИД подразделения");
            builder.Property(e => e.Name).HasColumnName("Название");
            builder.Property(e => e.Note).HasColumnName("Примечание");
        }
    }
}
