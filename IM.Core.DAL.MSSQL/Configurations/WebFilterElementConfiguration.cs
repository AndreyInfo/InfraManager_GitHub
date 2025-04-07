using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Settings;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public partial class WebFilterElementConfiguration : IEntityTypeConfiguration<WebFilterElement>
    {
        public void Configure(EntityTypeBuilder<WebFilterElement> entity)
        {
            entity.ToTable("WebFilterElements", "dbo");
            entity.HasKey(x => x.Id);
            entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedNever();
            entity.Property(e => e.Data).HasColumnName("Data").IsRequired();
        }
    }
}
