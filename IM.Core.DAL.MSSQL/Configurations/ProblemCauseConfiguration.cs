using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ProblemCauseConfiguration : LookupConfiguration<ProblemCause>
    {
        protected override string TableName => "ProblemCause";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<ProblemCause> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(500);
            builder.Property(x => x.ID).ValueGeneratedOnAdd();
        }
    }
}
