using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class SoftwareModelDependencyConfiguration : SoftwareModelDependencyConfigurationBase
{
    protected override string KeyName => "pk___software___c0850516_a2638_db5";

    protected override string ForeignKeyChildSoftwareModel => "fk_software_model_child_software_model_dependency";

    protected override string ForeignKeyParentSoftwareModel => "fk_software_model_parent_software_model_dependency";

    protected override void ConfigureDataBase(EntityTypeBuilder<SoftwareModelDependency> builder)
    {
        builder.ToTable("software_model_dependency", Options.Scheme);

        builder.Property(e => e.ParentSoftwareModelID).HasColumnName("parent_software_model_id");
        builder.Property(e => e.ChildSoftwareModelID).HasColumnName("child_software_model_id");
        builder.Property(e => e.Type).HasColumnName("type");
    }
}
