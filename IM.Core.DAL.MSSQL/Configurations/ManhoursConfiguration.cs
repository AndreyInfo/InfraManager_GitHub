using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.Manhours;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ManhoursConfiguration : ManhoursConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Manhours";

        protected override void ConfigureDatabase(EntityTypeBuilder<ManhoursEntry> builder)
        {
            builder.ToTable("Manhours", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.WorkID).HasColumnName("ManhoursWorkID");
            builder.Property(x => x.UtcDate).HasColumnName("UtcDate");
            builder.Property(x => x.Value).HasColumnName("ValueInMinutes");
        }
    }
}