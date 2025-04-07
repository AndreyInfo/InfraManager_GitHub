using IM.Core.DAL.Postgres;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class WebapiUsersConfiguration : IEntityTypeConfiguration<WebApiUsers>
    {
        public void Configure(EntityTypeBuilder<WebApiUsers> entity)
        {
            entity.ToView("webapi_users", Options.Scheme);

            entity.HasKey(e=>e.ImobjId).HasName("pk_webapi_users");

            entity.Property(e => e.Comments).HasColumnName("comments").HasMaxLength(100);

            entity.Property(e => e.DivisionId).HasColumnName("division_id");

            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100);

            entity.Property(e => e.ExternalId)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("external_id");

            entity.Property(e => e.Fax).HasColumnName("fax").HasMaxLength(100);

            entity.Property(e => e.ImobjId).HasColumnName("im_obj_id");

            entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100);

            entity.Property(e => e.Login).HasColumnName("login").HasMaxLength(100);

            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100);

            entity.Property(e => e.Pager).HasColumnName("pager").HasMaxLength(100);

            entity.Property(e => e.Patronymic).HasColumnName("patronymic").HasMaxLength(100);

            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(100);

            entity.Property(e => e.PositionId).HasColumnName("position_id");

            entity.Property(e => e.Removed).HasColumnName("removed");

            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.Property(e => e.WorkplaceId).HasColumnName("work_place_id");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<WebApiUsers> entity);
    }
}
