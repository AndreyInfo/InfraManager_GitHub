using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class DependencyConfigurationBase<TDependency> : IEntityTypeConfiguration<TDependency>
        where TDependency : Dependency
    {
        #region configuration

        public void Configure(EntityTypeBuilder<TDependency> builder)
        {
            builder.Property(x => x.ObjectClassID).IsRequired(true);
            builder.Property(x => x.ObjectID).IsRequired(true);
            builder.Property(x => x.ObjectName).IsRequired(true).HasMaxLength(1000);
            builder.Property(x => x.ObjectLocation).IsRequired(true).HasMaxLength(2000);
            builder.Property(x => x.Note).IsRequired(true).HasMaxLength(1000);
            builder.Property(x => x.Type).IsRequired(true);
            builder.Property(x => x.Locked).IsRequired(true);


            ConfigureSubtype(builder);
        }

        protected abstract void ConfigureSubtype(EntityTypeBuilder<TDependency> builder);
        protected abstract string PrimaryKeyName { get; }
        #endregion
    }
}
