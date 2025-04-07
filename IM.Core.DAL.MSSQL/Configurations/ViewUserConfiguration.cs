using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ViewUserConfiguration : IEntityTypeConfiguration<ViewUser>
    {
        public void Configure(EntityTypeBuilder<ViewUser> builder)
        {
            builder.ToView("view_User", "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.IntID)
                .HasColumnName("IntID");
            builder.Property(x => x.ID)
                .HasColumnName("ID");
            builder.Property(x => x.Family)
                .HasColumnName("Family");
            builder.Property(x => x.Name)
                .HasColumnName("Name");
            builder.Property(x => x.Patronymic)
                .HasColumnName("Patronymic");
            builder.Property(x => x.Phone)
                .HasColumnName("Phone");
            builder.Property(x => x.PhoneInternal)
                .HasColumnName("PhoneInternal");
            builder.Property(x => x.Email)
                .HasColumnName("Email");
            builder.Property(x => x.Fax)
                .HasColumnName("Fax");
            builder.Property(x => x.Pager)
                .HasColumnName("Pager");
            builder.Property(x => x.Note)
                .HasColumnName("Note");
            builder.Property(x => x.Login)
                .HasColumnName("Login");
            builder.Property(x => x.SID)
                .HasColumnName("SID");
            builder.Property(x => x.SDWebAccessIsGranted)
                .HasColumnName("SDWebAccessIsGranted");
            builder.Property(e => e.RowVersion)
                .IsRequired()
                .IsRowVersion();
        }
    }
}
