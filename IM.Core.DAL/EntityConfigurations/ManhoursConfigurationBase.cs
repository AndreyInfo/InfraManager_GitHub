using InfraManager.DAL.ServiceDesk.Manhours;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ManhoursConfigurationBase : IEntityTypeConfiguration<ManhoursEntry>
    {
        public void Configure(EntityTypeBuilder<ManhoursEntry> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
            builder.Property(x => x.ID).ValueGeneratedNever();

            ConfigureDatabase(builder);
        }

        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ManhoursEntry> builder);
    }
}