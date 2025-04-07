using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class CreepingLineConfiguration : IEntityTypeConfiguration<CreepingLine>
    {
        public void Configure(EntityTypeBuilder<CreepingLine> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.Name)
                .HasMaxLength(CreepingLine.MaxNameLength)
                .IsRequired(true);

            builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsRequired(true)
                .HasColumnType("timestamp");

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<CreepingLine> builder);

        protected abstract string PrimaryKeyName { get; }
    }
}
