using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ProblemTypeConfiguration : LookupConfiguration<ProblemType>
    {
        protected override string TableName => "problem_type";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<ProblemType> builder)
        {
            builder.Property(e => e.ID).HasColumnName("id").HasColumnType("uuid");
            builder.Property(x => x.Image).HasColumnName("icon");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("workflow_scheme_identifier")
                .HasMaxLength(250);
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(x => x.FormID).HasColumnName("form_id").HasDefaultValue(null);
            builder.Property(x => x.ParentProblemTypeID).HasColumnName("parent_problem_type_id");
            builder.Property(x => x.ImageName).HasColumnName("image_name");
            builder.Property(x => x.Removed).HasColumnName("removed");
            builder.IsMarkableForDelete();

            builder.HasOne(x => x.Parent)
                .WithMany()
                .HasForeignKey(c => c.ParentProblemTypeID)
                .HasConstraintName("fk_problem_type_problem_type");

            builder.HasOne(x => x.WorkflowScheme)
                .WithMany(x => x.ProblemTypes)
                .HasForeignKey(c => c.WorkflowSchemeIdentifier)
                .HasPrincipalKey(c => c.Identifier)
                .HasConstraintName("fk_problem_type_workflow_schem");

            builder.HasOne(x => x.Form)
              .WithMany()
              .HasForeignKey(c => c.FormID)
              .HasConstraintName("fk_problem_type_form")
              .IsRequired(false);
        }
    }
}