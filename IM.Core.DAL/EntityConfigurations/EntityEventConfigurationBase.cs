using InfraManager.DAL.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class EntityEventConfigurationBase : IEntityTypeConfiguration<EntityEvent>
    {
        public void Configure(EntityTypeBuilder<EntityEvent> builder)
        {
            DatabaseConfigure(builder);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Order).ValueGeneratedOnAdd();
        }

        protected abstract void DatabaseConfigure(EntityTypeBuilder<EntityEvent> builder);
    }
}
