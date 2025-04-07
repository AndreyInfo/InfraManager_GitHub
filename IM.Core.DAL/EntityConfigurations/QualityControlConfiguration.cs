using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Calls;
using InfraManager.DAL.ServiceDesk.Quality;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class QualityControlConfiguration : IEntityTypeConfiguration<QualityControl>
    {
        public void Configure(EntityTypeBuilder<QualityControl> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.UtcDate).IsRequired(true);
            builder.Property(x => x.Type).IsRequired(true);
            builder.Property(x => x.TimeSpanInWorkMinutes).IsRequired(true);
            builder.Property(x => x.TimeSpanInMinutes).IsRequired(true);

            builder.HasOne(x => x.Call)
                .WithMany()
                .HasForeignKey(x => x.CallID)
                .IsRequired();

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<QualityControl> builder);

        protected abstract string PrimaryKeyName { get; }

    }
}
