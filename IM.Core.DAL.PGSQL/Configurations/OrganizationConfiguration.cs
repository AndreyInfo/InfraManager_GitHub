using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.EntityConfigurations;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class OrganizationConfiguration : OrganizationConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_organization";

        protected override void ConfigureDataProvider(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("organization", Options.Scheme);
            
            builder.Property(e => e.ID).HasColumnName("identificator").HasDefaultValueSql("(gen_random_uuid())");
            builder.Property(e => e.CalendarWorkScheduleId).HasColumnName("calendar_work_schedule_id");
            builder.Property(e => e.ComplementaryId).HasColumnName("complementary_id");
            builder.Property(e => e.ExternalId).HasColumnName("external_id").HasDefaultValueSql("('')");
            builder.Property(e => e.IsLockedForOsi).HasColumnName("is_locked_for_osi");
            builder.Property(e => e.PeripheralDatabaseId).HasColumnName("peripheral_database_id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Note).HasColumnName("note");
        }
    }
}