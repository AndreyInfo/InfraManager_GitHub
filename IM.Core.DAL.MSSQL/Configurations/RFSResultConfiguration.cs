using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class RFSResultConfiguration : LookupConfiguration<RequestForServiceResult>
    {
        protected override string TableName => "RFSResult";

        protected override void ConfigureAdditionalMembers(
            EntityTypeBuilder<RequestForServiceResult> builder)
        {
            builder.IsMarkableForDelete();

            builder.Property(x => x.Name)
               .HasMaxLength(500);
        }
    }
}
