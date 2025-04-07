using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class IconNamesConfiguration:IconNameConfigurationBase
{
    protected override string PrimaryKeyName => "PK__IconName__3213E83F37991B3D";

    protected override void ConfigureDatabase(EntityTypeBuilder<Icon> builder)
    {
        builder.ToTable("Icons", "dbo");
        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("NEWID()");
        builder.Property(x => x.Name).HasColumnName("name");
    }
}