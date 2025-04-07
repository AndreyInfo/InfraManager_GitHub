using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class DeputyUserConfiguration : IEntityTypeConfiguration<DeputyUser>
    {
        public void Configure(EntityTypeBuilder<DeputyUser> builder)
        {
            builder.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);

            builder.IsMarkableForDelete();

            builder
               .HasOne(x => x.Child)
               .WithMany()
               .HasForeignKey(x => x.ChildUserId)
               .HasConstraintName(ChildUserForeignKeyName)
               .HasPrincipalKey(x => x.IMObjID);

            builder
                .HasOne(x => x.Parent)
                .WithMany()
                .HasForeignKey(x => x.ParentUserId)
                .HasConstraintName(ParentUserForeignKeyName)
                .HasPrincipalKey(x => x.IMObjID);

            builder.HasQueryFilter(DeputyUser.IsAlive);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<DeputyUser> builder);

        protected abstract string PrimaryKeyName { get; }

        protected abstract string ParentUserForeignKeyName { get; }

        protected abstract string ChildUserForeignKeyName { get; }
    }
}
