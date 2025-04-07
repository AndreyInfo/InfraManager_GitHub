using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkFlowSchemeConfigure : IEntityTypeConfiguration<WorkFlowScheme>
    {
        public void Configure(EntityTypeBuilder<WorkFlowScheme> builder)
        {
            builder.ToTable("WorkflowScheme", "dbo");
            builder.HasKey(x => x.Id);
            ConfigureProperty(builder);
            ConfigureForeignEntities(builder);
        }

        public void ConfigureProperty(EntityTypeBuilder<WorkFlowScheme> builder)
        {
            builder.Property(x => x.Identifier).HasMaxLength(200);
            builder.HasIndex(x => x.Identifier).IsUnique();
        }

        public void ConfigureForeignEntities(EntityTypeBuilder<WorkFlowScheme> builder)
        {
            builder.HasMany(c => c.ProblemTypes)
                    .WithOne(c => c.WorkflowScheme)
                    .HasForeignKey(c => c.WorkflowSchemeIdentifier)
                    .HasPrincipalKey(c => c.Identifier);
        }
    }
}
