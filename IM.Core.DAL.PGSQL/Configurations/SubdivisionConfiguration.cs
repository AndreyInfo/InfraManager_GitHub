using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.EntityConfigurations;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class SubdivisionConfiguration : SubdivisionConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_department";

        protected override string InxedUniqueName => "ui_subdivision_organization_parent_subdivision_name";

        protected override void ConfigureDataProvider(EntityTypeBuilder<Subdivision> builder)
        {
            builder.ToTable("department", Options.Scheme);
            
            builder.Property(e => e.ID).HasColumnName("identificator");
            builder.Property(e => e.CalendarWorkScheduleID).HasColumnName("calendar_work_schedule_id");
            builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
            builder.Property(e => e.ExternalID).HasColumnName("external_id").HasDefaultValueSql("('')");
            builder.Property(e => e.IsLockedForOsi).HasColumnName("is_locked_for_osi");
            builder.Property(e => e.OrganizationID).HasColumnName("organization_id");
            builder.Property(e => e.It).HasColumnName("i_t");
            builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
            builder.Property(e => e.SubdivisionID).HasColumnName("department_id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.Note).HasColumnName("note");
        }
    }
}