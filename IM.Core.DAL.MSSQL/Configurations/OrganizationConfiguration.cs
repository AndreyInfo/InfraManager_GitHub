using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class OrganizationConfiguration : OrganizationConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Организация";

        protected override void ConfigureDataProvider(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("Организация", "dbo");

            builder.Property(e => e.ID).HasColumnName("Идентификатор").HasDefaultValueSql("(newid())");
            builder.Property(e => e.CalendarWorkScheduleId).HasColumnName("CalendarWorkScheduleID");
            builder.Property(e => e.ComplementaryId).HasColumnName("ComplementaryID");
            builder.Property(e => e.ExternalId).HasColumnName("ExternalID").HasDefaultValueSql("('')");
            builder.Property(e => e.IsLockedForOsi).HasColumnName("IsLockedForOSI");
            builder.Property(e => e.PeripheralDatabaseId).HasColumnName("PeripheralDatabaseID");
            builder.Property(e => e.Name).HasColumnName("Название");
            builder.Property(e => e.Note).HasColumnName("Примечание");
        }
    }
}
