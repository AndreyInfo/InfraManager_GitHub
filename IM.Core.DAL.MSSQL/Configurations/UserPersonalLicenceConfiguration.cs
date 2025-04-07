using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class UserPersonalLicenceConfiguration : UserPersonalLicenceConfigurationBase
{
    protected override string PK_Name => "PK_UserPersonalLicence";
    protected override string UI_User_ID => "UI_UserPersonalLicences_UserID";

    protected override void ConfigureDataBase(EntityTypeBuilder<UserPersonalLicence> builder)
    {
        builder.ToTable("UserPersonalLicence", Options.Scheme);

        builder.Property(x => x.UserID).HasColumnName("UserID");
        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.UtcDateCreated).HasColumnName("UtcDateCreated");
    }
}