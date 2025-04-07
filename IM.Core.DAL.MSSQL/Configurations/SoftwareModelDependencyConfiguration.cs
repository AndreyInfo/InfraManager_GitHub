using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class SoftwareModelDependencyConfiguration : SoftwareModelDependencyConfigurationBase
{
    protected override string KeyName => "PK__Software__C0850516A2638DB5";

    protected override string ForeignKeyChildSoftwareModel => "FK_SoftwareModel_ChildSoftwareModelDependency";

    protected override string ForeignKeyParentSoftwareModel => "FK_SoftwareModel_ParentSoftwareModelDependency";

    protected override void ConfigureDataBase(EntityTypeBuilder<SoftwareModelDependency> builder)
    {
        builder.ToTable("SoftwareModelDependency", "dbo");

        builder.Property(e => e.ParentSoftwareModelID).HasColumnName("ParentSoftwareModelID");
        builder.Property(e => e.ChildSoftwareModelID).HasColumnName("ChildSoftwareModelID");
        builder.Property(e => e.Type).HasColumnName("Type");
    }
}
