using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class SessionConfiguration : SessionConfigurationBase
{
    protected override string PK_Name => "PK_UserSession";
    protected override void ConfigureDataBase(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("UserSession", Options.Scheme);
        
        builder.Property(x => x.SecurityStamp).HasColumnName("SecurityStamp");
        builder.Property(x => x.UserAgent).HasColumnName("UserAgent");
        builder.Property(x => x.UserID).HasColumnName("UserID");
        builder.Property(x => x.UtcDateClosed).HasColumnType("datetime").HasColumnName("UtcDateClosed");
        builder.Property(x => x.UtcDateOpened).HasColumnType("datetime").HasColumnName("UtcDateOpened");
        builder.Property(x => x.UtcDateLastActivity).HasColumnType("datetime").HasColumnName("UtcDateLastActivity");
    }
}