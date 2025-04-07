using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class UserActivityTypeConfiguration : LookupConfiguration<UserActivityType>
    {
        protected override string TableName => "user_activity_type";


        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<UserActivityType> builder)
        {
            builder.ToTable("user_activity_type", Options.Scheme);
            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(e => e.ParentID).HasColumnName("parent_id");
            builder.Property(e => e.Removed).HasColumnName("removed");
            builder.HasKey(x => x.ID).HasName("pk_user_activity_type");

            builder.HasMany(x => x.Childs).WithOne().HasForeignKey(x => x.ParentID);

            builder.HasOne(x => x.Parent)
                .WithMany(t => t.Childs)
                .HasForeignKey(c => c.ParentID);

            builder.IsMarkableForDelete();
        }
    }
}