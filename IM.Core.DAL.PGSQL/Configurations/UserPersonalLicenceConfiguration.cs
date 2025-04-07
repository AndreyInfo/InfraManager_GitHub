using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class UserPersonalLicenceConfiguration : UserPersonalLicenceConfigurationBase
{
    protected override string PK_Name => "pk_user_personal_licence";
    protected override string UI_User_ID => "ui_personal_licence_user_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<UserPersonalLicence> builder)
    {
        builder.ToTable("user_personal_licence", Options.Scheme);

        builder.Property(x => x.UserID).HasColumnName("user_id");
        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.UtcDateCreated).HasColumnName("utc_date_created");
    }
}