using InfraManager.DAL.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SessionConfigurationBase : IEntityTypeConfiguration<Session>
{
    protected abstract string PK_Name { get; }
    
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(x => new { x.UserID, x.UserAgent }).HasName(PK_Name);
        
        builder.Property(x => x.SecurityStamp).HasMaxLength(250);
        builder.Property(x => x.UserAgent).HasMaxLength(400);
        builder.Property(x => x.UtcDateClosed).IsRequired(false);
        
        builder.HasOne(x => x.User).WithOne().HasForeignKey<Session>(x => x.UserID)
            .HasPrincipalKey<User>(x => x.IMObjID);
        
        ConfigureDataBase(builder);
    }
    
    protected abstract void ConfigureDataBase(EntityTypeBuilder<Session> builder);
}