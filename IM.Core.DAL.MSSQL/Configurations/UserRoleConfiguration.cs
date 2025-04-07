using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole", "dbo");
            builder.HasKey(x => new { x.RoleID, x.UserID });

            builder.Property(c => c.RoleID).HasColumnName("RoleID");
            builder.Property(c => c.UserID).HasColumnName("UserID");

            builder.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleID)
                .HasConstraintName("FK_UserRole_Role");
        }
    }
}
