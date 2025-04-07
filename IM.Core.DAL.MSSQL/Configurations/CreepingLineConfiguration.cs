using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CreepingLineConfiguration : EntityConfigurations.CreepingLineConfiguration
    {
        protected override string PrimaryKeyName => "PK_CreepingLine";

        protected override void ConfigureDatabase(EntityTypeBuilder<CreepingLine> builder)
        {
            builder.ToTable("CreepingLine", "dbo");

            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .HasDefaultValueSql("NEWID()")
                .HasColumnName("ID");

            builder.Property(x => x.Name)
                .HasColumnName("Name");

            builder.Property(x => x.Visible)
              .HasColumnName("Visible");

            builder.Property(x => x.RowVersion)
                .HasColumnName("RowVersion");

        }
    }
}
