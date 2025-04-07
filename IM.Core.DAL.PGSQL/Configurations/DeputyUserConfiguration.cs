using IM.Core.DAL.Postgres;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class DeputyUserConfiguration : InfraManager.DAL.EntityConfigurations.DeputyUserConfiguration
    {
        protected override string PrimaryKeyName => "pk_deputy_user";

        protected override string ParentUserForeignKeyName => "fk_deputy_user_parent";

        protected override string ChildUserForeignKeyName => "fk_deputy_user_child";

        protected override void ConfigureDatabase(EntityTypeBuilder<DeputyUser> builder)
        {
            builder.ToTable("deputy_user", Options.Scheme);

            builder.Property(x => x.IMObjID).HasColumnName("id");
            builder.Property(x => x.ChildUserId).HasColumnName("child_user_id");
            builder.Property(x => x.ParentUserId).HasColumnName("parent_user_id");
            builder.Property(x => x.UtcDataDeputyBy).HasColumnName("utc_data_deputy_by").HasColumnType("timestamp(3)");
            builder.Property(x => x.UtcDataDeputyWith).HasColumnName("utc_data_deputy_with")
                .HasColumnType("timestamp(3)");
            builder.Property(x => x.Removed).HasColumnName("removed");
        }
    }
}