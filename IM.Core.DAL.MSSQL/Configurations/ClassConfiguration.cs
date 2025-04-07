using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ClassConfiguration : InframanagerClassConfigurationBase
    {
        protected override void ConfigureDatabase(EntityTypeBuilder<InframanagerObjectClass> builder)
        {
            builder.ToTable("Class", "dbo");

            builder.Property(e => e.ID).HasColumnName("ClassID");
            builder.Property(e => e.Name).HasColumnName("Name");
        }
    }
}
