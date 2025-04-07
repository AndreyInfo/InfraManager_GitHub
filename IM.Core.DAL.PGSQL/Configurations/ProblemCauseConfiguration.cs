using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ProblemCauseConfiguration : LookupConfiguration<ProblemCause>
    {
        protected override string TableName => "problem_cause";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<ProblemCause> builder)
        {
            builder.Property(e => e.Removed).HasColumnName("removed");
            builder.HasXminRowVersion(e => e.RowVersion);
        }
    }
}