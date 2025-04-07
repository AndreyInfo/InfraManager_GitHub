using System.Data;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SLAServiceReferenceConfigurationBase : IEntityTypeConfiguration<SLAServiceReference>
{
    public void Configure(EntityTypeBuilder<SLAServiceReference> builder)
    {
        builder.HasKey(x => new { x.SLAID, x.ServiceReferenceID }); //TODO в базе нет такого ключа, но без него ef core не хочет удалять запись.
                                                                    //По хорошему добавить данный ключ, чтобы через миграции или код ферст добавился данный ключ
        
        ConfigureDatabase(builder);
    }
    
    protected abstract void ConfigureDatabase(EntityTypeBuilder<SLAServiceReference> builder);
}