using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class GroupUserConfiguration : IEntityTypeConfiguration<GroupUser>
    {
        public void Configure(EntityTypeBuilder<GroupUser> builder)
        {
            builder.ToTable("QueueUser", "dbo");
            builder.HasKey(x => new { x.GroupID, x.UserID });

            builder.Property(x => x.GroupID).HasColumnName("QueueID").IsRequired();
            builder.Property(x => x.UserID).HasColumnName("UserID").IsRequired();

            builder
                .HasOne(bc => bc.User)
                .WithMany()
                .HasForeignKey(bc => bc.UserID)
                .HasPrincipalKey(x => x.IMObjID);

        }
    }
}
