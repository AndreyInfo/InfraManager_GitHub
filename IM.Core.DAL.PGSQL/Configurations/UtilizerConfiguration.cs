using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class UtilizerConfiguration : UtilizerConfigurationBase
{
    private const string UtilizerViewName = "view_utilizer";

    protected override void ConfigureDbProvider(EntityTypeBuilder<Utilizer> builder)
    {
        builder
            .ToView(UtilizerViewName);
    }
}
