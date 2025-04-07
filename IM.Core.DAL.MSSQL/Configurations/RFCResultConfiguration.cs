using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class RFCResultConfiguration : LookupConfiguration<RequestForServiceResult>
{
    protected override string TableName => "RFSResult";

    protected override void ConfigureAdditionalMembers(
        EntityTypeBuilder<RequestForServiceResult> builder)
    {
        builder.HasKey(e => e.ID).HasName("PK_RFSResult");

        builder.HasIndex(e => e.Name, "UI_Name_RfsResult").IsUnique();

        builder.Property(x => x.Removed).HasColumnName("Removed");
        builder.IsMarkableForDelete();
    }
}
