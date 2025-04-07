using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class SupportLineResponsibleConfigurationBase : IEntityTypeConfiguration<SupportLineResponsible>
{
    protected abstract string KeyName { get; }
    public void Configure(EntityTypeBuilder<SupportLineResponsible> builder)
    {
        builder.HasKey(c => new { c.ObjectID, c.LineNumber }).HasName(KeyName);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<SupportLineResponsible> builder);
}
