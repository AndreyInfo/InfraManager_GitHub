using IM.Core.DAL.Postgres;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class GroupUserConfiguration : IEntityTypeConfiguration<GroupUser>
    {
        public void Configure(EntityTypeBuilder<GroupUser> builder)
        {
            builder.ToTable("queue_user", Options.Scheme);

            builder.HasKey(x => new {x.GroupID, x.UserID}).HasName("pk_queue_user");

            builder.Property(x => x.GroupID).HasColumnName("queue_id").IsRequired();
            builder.Property(x => x.UserID).HasColumnName("user_id").IsRequired();

            builder
                .HasOne(bc => bc.User)
                .WithMany()
                .HasForeignKey(bc => bc.UserID)
                .HasPrincipalKey(x => x.IMObjID);
        }
    }
}