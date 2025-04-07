using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ClassIconConfiguration : IEntityTypeConfiguration<ClassIcon>
    {
        public void Configure(EntityTypeBuilder<ClassIcon> builder)
        {
            builder.ToTable("ClassIcon", "dbo");

            builder.HasKey(c => c.ClassID);
            builder.Property(c => c.IconName).HasMaxLength(200);
        }
    }
}
