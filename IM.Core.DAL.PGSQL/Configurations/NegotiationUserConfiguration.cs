using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class NegotiationUserConfiguration : NegotiationUserConfigurationBase
    {
        protected override string UserForeignKeyConstraint => "fk_negotiation_user_negotiation";

        protected override void ConfigureDatabase(EntityTypeBuilder<NegotiationUser> builder)
        {
            builder.ToTable("negotiation_user", Options.Scheme);

            builder.Property(x => x.Message).HasColumnName("message");
            builder.Property(x => x.NegotiationID).HasColumnName("negotiation_id");
            builder.Property(x => x.OldUserName).HasColumnName("old_user_name");
            builder.Property(x => x.UserID).HasColumnName("user_id");
            builder.Property(x => x.UtcDateComment).HasColumnName("utc_date_comment");
            builder.Property(x => x.UtcVoteDate).HasColumnName("utc_date_vote");
            builder.Property(x => x.VotingType).HasColumnName("voting_type");
        }
    }
}