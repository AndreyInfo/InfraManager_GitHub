using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.PGSQL.Configurations
{
    internal class ClassIconConfiguration : IEntityTypeConfiguration<ClassIcon>
    {
        public void Configure(EntityTypeBuilder<ClassIcon> builder)
        {
            builder.ToTable("class_icon", Options.Scheme);
            builder.HasKey(c => c.ClassID);
            builder.Property(c => c.ClassID).HasColumnName("class_id");
            builder.Property(c => c.IconName).HasColumnName("icon_name");
        }
    }
}