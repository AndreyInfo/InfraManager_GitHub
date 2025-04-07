using Inframanager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class RFCTypeConfiguration : RFCTypeConfigurationBase
    {
        protected override string IndexName => "UI_RFCType_Name";

        protected override void ConfigureDatabase(EntityTypeBuilder<ChangeRequestType> builder)
        {
            builder.ToTable("RfcType", "dbo");

            builder.Property(x => x.RowVersion).IsRowVersion();
            builder.Property(x => x.Icon).HasColumnType("image");
        }
    }
}
