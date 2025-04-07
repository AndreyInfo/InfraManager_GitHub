using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class NegotiationUserConfigurationBase : IEntityTypeConfiguration<NegotiationUser>
    {
        public void Configure(EntityTypeBuilder<NegotiationUser> builder)
        {
            builder.HasKey(x => new { x.NegotiationID, x.UserID });
            builder.Property(x => x.Message).IsRequired().HasMaxLength(4000);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserID)
                .HasPrincipalKey(u => u.IMObjID)
                .HasConstraintName(UserForeignKeyConstraint);

            ConfigureDatabase(builder);
        }

        protected abstract string UserForeignKeyConstraint { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<NegotiationUser> builder);
    }
}
