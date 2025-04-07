using InfraManager.DAL.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class UserSessionHistoryConfigurationBase : IEntityTypeConfiguration<UserSessionHistory>
{
    protected abstract string PK_Name { get; }
    
    public void Configure(EntityTypeBuilder<UserSessionHistory> builder)
    {
        builder.HasKey(x => x.ID).HasName(PK_Name);
        
        builder.Property(x => x.UserAgent).HasMaxLength(400);

        builder.HasOne(x => x.User).WithOne().HasForeignKey<UserSessionHistory>(x => x.UserID)
            .HasPrincipalKey<User>(x => x.IMObjID);
        
        builder.HasOne(x => x.Executor).WithOne().HasForeignKey<UserSessionHistory>(x => x.ExecutorID)
            .HasPrincipalKey<User>(x => x.IMObjID);
        
        ConfigureDataBase(builder);
    }
    
    protected abstract void ConfigureDataBase(EntityTypeBuilder<UserSessionHistory> builder);
}