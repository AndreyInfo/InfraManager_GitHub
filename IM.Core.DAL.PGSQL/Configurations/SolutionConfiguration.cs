using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Postgres;

namespace IM.Core.DAL.Postgres.Configurations;

internal sealed class SolutionConfiguration : SolutionConfigurationBase
{
    protected override string PrimaryKeyName => "pk_solution";

    protected override string UIName => "ui_name_solution";

    protected override void ConfigureDatabase(EntityTypeBuilder<Solution> builder)
    {
        builder.ToTable("solution", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasColumnType("uuid");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Description).HasColumnName("description").HasColumnType("text");
        builder.Property(x => x.HTMLDescription).HasColumnName(@"html_description").HasColumnType("text");
        builder.HasXminRowVersion(c => c.RowVersion);
    }
}
