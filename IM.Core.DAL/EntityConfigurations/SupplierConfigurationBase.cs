using InfraManager.DAL.Finance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class SupplierConfigurationBase : IEntityTypeConfiguration<Supplier>
    {
        protected abstract string PrimaryKeyName { get; }

        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.Address).HasMaxLength(510);
            builder.Property(x => x.Email).HasMaxLength(510);
            builder.Property(x => x.ExternalID).HasMaxLength(500);
            builder.Property(x => x.Inn).HasMaxLength(50);
            builder.Property(x => x.Kpp).HasMaxLength(50);
            builder.Property(x => x.Name).HasMaxLength(510);
            builder.Property(x => x.Notice).HasMaxLength(510);
            builder.Property(x => x.Phone).HasMaxLength(510);
            builder.Property(x => x.RegisteredAddress).HasMaxLength(510);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<Supplier> builder);
    }
}