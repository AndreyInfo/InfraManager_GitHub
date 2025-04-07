using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

internal sealed class SolutionConfiguration : SolutionConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Solution";

    protected override string UIName => "UI_Name_Solution";

    protected override void ConfigureDatabase(EntityTypeBuilder<Solution> builder)
    {
        builder.ToTable("Solution", "dbo");

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.HTMLDescription).HasColumnName("HTMLDescription");
        builder.Property(x => x.Description).HasColumnName("Description");
        builder.Property(x => x.RowVersion)
            .HasColumnName("RowVersion")
            .HasColumnType("timestamp")
            .IsRowVersion();
    }
}
