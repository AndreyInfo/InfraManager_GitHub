using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ObjectIconConfigurationBase : IEntityTypeConfiguration<ObjectIcon>
    {
        public void Configure(EntityTypeBuilder<ObjectIcon> builder)
        {
            builder.HasKey(x => x.ID);
            builder
                .HasIndex(x => new { x.ObjectID, x.ObjectClassID })
                .IsUnique()
                .HasDatabaseName(UniqueKeyObjectIDName);
            builder.Property(x => x.ID).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.Content).IsRequired(false);

            ConfigureDataProvider(builder);
        }

        protected abstract string UniqueKeyObjectIDName { get; }

        protected abstract void ConfigureDataProvider(EntityTypeBuilder<ObjectIcon> builder);
    }
}
