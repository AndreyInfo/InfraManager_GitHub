using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ProblemTypeConfiguration : LookupConfiguration<ProblemType>
    {
        protected override string TableName => "ProblemType";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<ProblemType> builder)
        {
            builder.Property(x => x.Image).HasColumnName("Icon");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasMaxLength(250);
            builder.Property(x => x.FormID).HasColumnName("FormID").HasDefaultValue(null);
            builder.Property(x => x.ParentProblemTypeID);
            builder.Property(x => x.ImageName).HasColumnName("ImageName");
            builder.IsMarkableForDelete();

            builder.HasOne(x => x.Parent)
                .WithMany()
                .HasForeignKey(c=> c.ParentProblemTypeID)
                .HasConstraintName("FK_ProblemType_ProblemType");
            
            builder.HasOne(x => x.WorkflowScheme)
                .WithMany(x=> x.ProblemTypes)
                .HasForeignKey(c => c.WorkflowSchemeIdentifier)
                .HasPrincipalKey(c=> c.Identifier)
                .HasConstraintName("FK_ProblemType_WorkflowSchem");

            builder.HasOne(x => x.Form)
              .WithMany()
              .HasForeignKey(c => c.FormID)
              .HasConstraintName("FK_ProblemType_Form")
              .IsRequired(false);
        }
    }
}