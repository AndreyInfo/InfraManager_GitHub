using InfraManager.DAL.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class UserPersonalLicenceConfigurationBase : IEntityTypeConfiguration<UserPersonalLicence>
{
    protected abstract string PK_Name { get; }
    protected abstract string UI_User_ID { get; }
    
    public void Configure(EntityTypeBuilder<UserPersonalLicence> builder)
    {
        builder.HasKey(x => x.ID).HasName("pk_user_personal_session").HasName(PK_Name);
        
        builder.HasOne(x => x.User).WithOne().HasForeignKey<UserPersonalLicence>(x => x.UserID)
            .HasPrincipalKey<User>(x => x.IMObjID);

        builder.HasIndex(x => x.UserID, UI_User_ID).IsUnique();
        ConfigureDataBase(builder);
    }
    
    protected abstract void ConfigureDataBase(EntityTypeBuilder<UserPersonalLicence> builder);
}