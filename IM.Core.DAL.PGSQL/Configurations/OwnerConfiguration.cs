using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.PGSQL.Configurations
{
    internal class OwnerConfiguration : OwnerConfigurationBase
    {
        protected override void ConfigureDataProvider(EntityTypeBuilder<Owner> builder)
        {
            builder.ToTable("owner", Options.Scheme);

            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.VisioID).HasColumnName("visio_id");
            builder.Property(x => x.IMObjID).HasColumnName("im_obj_id");
        }
    }
}