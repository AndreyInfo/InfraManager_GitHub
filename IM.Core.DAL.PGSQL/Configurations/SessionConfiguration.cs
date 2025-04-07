using System.Data;
using InfraManager.DAL;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class SessionConfiguration : SessionConfigurationBase
{
    protected override string PK_Name => "pk_user_session";
    
    protected override void ConfigureDataBase(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("user_session", Options.Scheme);
        
        builder.Property(x => x.SecurityStamp).HasColumnName("security_stamp");
        builder.Property(x => x.UserAgent).HasColumnName("user_agent");
        builder.Property(x => x.UserID).HasColumnName("user_id");
        builder.Property(x => x.UtcDateClosed).HasColumnName("utc_date_closed");
        builder.Property(x => x.UtcDateOpened).HasColumnName("utc_date_opened");
        builder.Property(x => x.UtcDateLastActivity).HasColumnName("utc_date_last_activity");
        builder.Property(x => x.Location).HasColumnName("location");
        builder.Property(x => x.LicenceType).HasColumnName("licence_type");
    }
}