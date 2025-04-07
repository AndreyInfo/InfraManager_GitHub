using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ProblemDependencyConfigurationBase : DependencyConfigurationBase<ProblemDependency>
    {
        protected abstract string ProblemForeignKey { get; }
        protected abstract string UniqueKeyName { get; }
        protected abstract string OwnerObjectIDIndexName { get; }

        protected override void ConfigureSubtype(EntityTypeBuilder<ProblemDependency> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.HasOne<Problem>()
                .WithMany(x => x.Dependencies)
                .HasForeignKey(x => x.OwnerObjectID)
                .HasPrincipalKey(x => x.IMObjID)
                .HasConstraintName(ProblemForeignKey);

            builder.HasIndex(x => x.OwnerObjectID)
                .HasDatabaseName(OwnerObjectIDIndexName);

            builder.HasIndex(x => new { x.OwnerObjectID, x.ObjectID, x.ObjectClassID, })
                .IsUnique(true)
                .HasDatabaseName(UniqueKeyName);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ProblemDependency> builder);
    }
}
