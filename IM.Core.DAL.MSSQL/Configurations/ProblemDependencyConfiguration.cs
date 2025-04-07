using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ProblemDependencyConfiguration : ProblemDependencyConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ProblemDependency";
        protected override string ProblemForeignKey => "FK_Problem_ProblemDependency";
        protected override string UniqueKeyName => "UX_ProblemDependency";
        protected override string OwnerObjectIDIndexName => "IX_ProblemDependency_ProblemID";

        protected override void ConfigureDatabase(
            EntityTypeBuilder<ProblemDependency> builder)
        {
            builder.ToTable("ProblemDependency", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Locked).HasColumnName("Locked");
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
            builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
            builder.Property(x => x.ObjectLocation).HasColumnName("ObjectLocation");
            builder.Property(x => x.ObjectName).HasColumnName("ObjectName");
            builder.Property(x => x.OwnerObjectID).HasColumnName("ProblemID");
            builder.Property(x => x.Type).HasColumnName("Type");
        }
    }
}
