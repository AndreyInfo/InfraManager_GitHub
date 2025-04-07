using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ChangeRequestDependencyConfigurationBase : DependencyConfigurationBase<ChangeRequestDependency>
    {
        #region configuration


        protected override void ConfigureSubtype(EntityTypeBuilder<ChangeRequestDependency> builder)
        {
            builder.HasKey(x => new { x.OwnerObjectID, x.ObjectID }).HasName(PrimaryKeyName);
            builder.Property(x => x.OwnerObjectID).IsRequired(true);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ChangeRequestDependency> builder);

        #endregion
    }
}
