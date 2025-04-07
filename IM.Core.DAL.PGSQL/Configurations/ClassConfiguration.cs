using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.PGSQL.Configurations
{
    internal class ClassConfiguration : InframanagerClassConfigurationBase
    {
        protected override void ConfigureDatabase(EntityTypeBuilder<InframanagerObjectClass> builder)
        {
            builder.ToTable("class", Options.Scheme);

            builder.Property(e => e.ID).HasColumnName("class_id");
            builder.Property(e => e.Name).HasColumnName("name");
        }
    }
}