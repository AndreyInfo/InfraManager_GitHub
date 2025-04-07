using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class UserSessionHistoryConfiguration : UserSessionHistoryConfigurationBase
{
    protected override string PK_Name => "pk_user_session_history";

    protected override void ConfigureDataBase(EntityTypeBuilder<UserSessionHistory> builder)
    {
        builder.ToTable("user_session_history", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.UserID).HasColumnName("user_id");
        builder.Property(x => x.Type).HasColumnName("type");
        builder.Property(x => x.UtcDate).HasColumnName("utc_date");
        builder.Property(x => x.ExecutorID).HasColumnName("executor_id");
        builder.Property(x => x.UserAgent).HasColumnName("user_agent");
    }
}