using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class OwnerConfiguration : OwnerConfigurationBase
    {
        protected override void ConfigureDataProvider(EntityTypeBuilder<Owner> builder)
        {
            builder.ToTable("Владелец", "dbo");

            builder.Property(x => x.IMObjID).HasColumnName("IMObjID");
            builder.Property(x => x.Name).HasColumnName("Название");
            builder.Property(x => x.VisioID).HasColumnName("Visio_ID");
        }
    }
}
