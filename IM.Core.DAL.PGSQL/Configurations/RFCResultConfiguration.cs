using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class RFCResultConfiguration : LookupConfiguration<RequestForServiceResult>
{
    protected override string TableName => "rfs_result";

    protected override void ConfigureAdditionalMembers(
        EntityTypeBuilder<RequestForServiceResult> builder)
    {
        builder.HasKey(e => e.ID).HasName("pk_rfs_result");

        builder.HasIndex(e => e.Name, "ui_name_rfs_result").IsUnique();

        builder.Property(x => x.Removed).HasColumnName("removed");
        builder.IsMarkableForDelete();
    }
}