using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class WorkorderDependencyConfigurationBase : DependencyConfigurationBase<WorkorderDependency>
    {
        #region configuration


        protected override void ConfigureSubtype(EntityTypeBuilder<WorkorderDependency> builder)
        {
            builder.HasKey(x => new { x.OwnerObjectID, x.ObjectID }).HasName(PrimaryKeyName);
            builder.Property(x => x.OwnerObjectID).IsRequired();

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<WorkorderDependency> builder);

        #endregion
    }
}
