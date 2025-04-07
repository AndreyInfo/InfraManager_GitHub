using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class DeputyUserConfiguration : EntityConfigurations.DeputyUserConfiguration
    {
        protected override string PrimaryKeyName => "PK_DeputyUser";

        protected override string ParentUserForeignKeyName => "FK_DeputyUser_ParentUser";

        protected override string ChildUserForeignKeyName => "FK_DeputyUser_ChildUser";

        protected override void ConfigureDatabase(EntityTypeBuilder<DeputyUser> builder)
        {
            builder.ToTable("DeputyUser", "dbo");

            builder.Property(x => x.IMObjID).HasColumnName("ID");
            builder.Property(x => x.ChildUserId).HasColumnName("ChildUserID");
            builder.Property(x => x.ParentUserId).HasColumnName("ParentUserID");
            builder.Property(x => x.UtcDataDeputyBy).HasColumnName("UtcDataDeputyBy").HasColumnType("datetime");
            builder.Property(x => x.UtcDataDeputyWith).HasColumnName("UtcDataDeputyWith").HasColumnType("datetime");
            builder.Property(x => x.Removed).HasColumnName("Removed");
        }
    }
}