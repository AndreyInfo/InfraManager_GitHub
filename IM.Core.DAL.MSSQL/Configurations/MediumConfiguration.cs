using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class MediumConfiguration : MediumConfigurationBase
{
    protected override string PrimaryKey => "PK_Виды среды передачи";

    protected override void ConfigureDatabase(EntityTypeBuilder<Medium> builder)
    {
        builder.ToTable("Виды среды передачи", Options.Scheme);

        builder.Property(i => i.ID).HasColumnName("Идентификатор");
        builder.Property(i => i.Name).HasColumnName("Название");
    }
}
