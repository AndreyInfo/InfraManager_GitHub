using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class CallDependencyConfigurationBase : DependencyConfigurationBase<CallDependency>
    {
        #region configuration

        protected override void ConfigureSubtype(EntityTypeBuilder<CallDependency> builder)
        {
            builder.HasKey(x => new { x.OwnerObjectID, x.ObjectID }).HasName(PrimaryKeyName);
            builder.Property(x => x.OwnerObjectID).IsRequired(true);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<CallDependency> builder);

        #endregion
    }
}
