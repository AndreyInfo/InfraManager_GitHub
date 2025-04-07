using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class UserSessionHistoryConfiguration : UserSessionHistoryConfigurationBase
{
    protected override string PK_Name => "PK_UserSessionHistory";
    
    protected override void ConfigureDataBase(EntityTypeBuilder<UserSessionHistory> builder)
    {
        builder.ToTable("UserSessionHistory", Options.Scheme);
        
        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.UserID).HasColumnName("UserID");
        builder.Property(x => x.Type).HasColumnName("Type");
        builder.Property(x => x.UtcDate).HasColumnName("UtcDate");
        builder.Property(x => x.ExecutorID).HasColumnName("ExecutorID");
        builder.Property(x => x.UserAgent).HasColumnName("UserAgent");
    }
}