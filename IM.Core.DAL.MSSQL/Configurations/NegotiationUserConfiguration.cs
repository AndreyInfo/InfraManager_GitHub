using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class NegotiationUserConfiguration : NegotiationUserConfigurationBase
    {
        protected override string UserForeignKeyConstraint => "FK_NegotiationUser_Negotiation";

        protected override void ConfigureDatabase(EntityTypeBuilder<NegotiationUser> builder)
        {
            builder.ToTable("NegotiationUser", "dbo");

            builder.Property(x => x.Message).HasColumnName("Message");
            builder.Property(x => x.NegotiationID).HasColumnName("NegotiationID");
            builder.Property(x => x.OldUserName).HasColumnName("OldUserName");
            builder.Property(x => x.UserID).HasColumnName("UserID");
            builder.Property(x => x.UtcDateComment).HasColumnName("UtcDateComment").HasColumnType("datetime");
            builder.Property(x => x.UtcVoteDate).HasColumnName("UtcDateVote").HasColumnType("datetime");
            builder.Property(x => x.VotingType).HasColumnName("VotingType");
        }
    }
}
