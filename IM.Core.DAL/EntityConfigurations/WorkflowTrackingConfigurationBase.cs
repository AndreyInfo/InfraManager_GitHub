using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class WorkflowTrackingConfigurationBase : IEntityTypeConfiguration<WorkflowTracking>
{
    protected abstract string KeyName { get; }

    public void Configure(EntityTypeBuilder<WorkflowTracking> builder)
    {
        builder.HasKey(x => x.ID).HasName(KeyName);

        builder.Property(x => x.WorkflowSchemeIdentifier).IsRequired(true);
        builder.Property(x => x.WorkflowSchemeVersion).IsRequired(true);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<WorkflowTracking> builder);
}
