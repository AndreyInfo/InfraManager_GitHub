using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class IncidentResultConfiguration : LookupConfiguration<IncidentResult>
{
    protected override string TableName => "IncidentResult";

    protected override void ConfigureAdditionalMembers(EntityTypeBuilder<IncidentResult> builder)
    {
        builder.HasIndex(c => c.Name, "UI_Name_IncidentResult").IsUnique();

        builder.IsMarkableForDelete();

        builder.Property(x => x.Name)
           .HasMaxLength(500);
    }
}
