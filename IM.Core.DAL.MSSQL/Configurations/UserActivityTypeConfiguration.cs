using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class UserActivityTypeConfiguration : LookupConfiguration<UserActivityType>
    {
        protected override string TableName => "UserActivityType";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<UserActivityType> builder)
        {
            builder.Property(x => x.ParentID)
                .HasColumnName("ParentID");

            builder.Property(x => x.RowVersion)
                .IsRequired();

            builder.Property(e => e.Removed)
                .HasColumnName("Removed");

            builder
                .HasMany(x => x.Childs).WithOne()
                .HasForeignKey(x => x.ParentID);

            builder.HasKey(x => x.ID).HasName("PK_UserActivityType");

            builder.IsMarkableForDelete();

            builder.HasOne(x => x.Parent)
                .WithMany(t => t.Childs)
                .HasForeignKey(c => c.ParentID);
        }
    }
}
