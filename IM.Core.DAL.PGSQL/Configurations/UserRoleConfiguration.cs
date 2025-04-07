using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_role");

            builder.HasKey(x => new { x.RoleID, x.UserID }).HasName("pk_user_role");

            builder.Property(c => c.RoleID).HasColumnName("role_id");
            builder.Property(c => c.UserID).HasColumnName("user_id");

            builder.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleID)
                .HasConstraintName("fk_user_role_role");
        }
    }
}