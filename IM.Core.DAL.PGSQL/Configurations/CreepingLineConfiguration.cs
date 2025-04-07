using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class CreepingLineConfiguration : EntityConfigurations.CreepingLineConfiguration
    {
        protected override string PrimaryKeyName => "pk_creeping_line";

        protected override void ConfigureDatabase(EntityTypeBuilder<CreepingLine> builder)
        {
            builder.ToTable("creeping_line", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.Visible).HasColumnName("visible");
            builder.Property(x => x.RowVersion).HasColumnName("row_version");

            builder.HasXminRowVersion(e => e.RowVersion);
        }
    }
}