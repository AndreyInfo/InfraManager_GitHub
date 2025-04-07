using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class IncidentResultConfiguration : LookupConfiguration<IncidentResult>
{
    protected override string TableName => "incident_result";

    protected override void ConfigureAdditionalMembers(EntityTypeBuilder<IncidentResult> builder)
    {
        builder.HasIndex(c => c.Name, "ui_name_incident_result").IsUnique();

        builder.Property(e => e.ID).HasColumnName("id");
        builder.HasXminRowVersion(e => e.RowVersion);

        builder.HasKey(e => e.ID).HasName("pk_incident_result");

        builder.Property(x => x.Removed).HasColumnName("removed");
        builder.IsMarkableForDelete();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(500);
    }
}