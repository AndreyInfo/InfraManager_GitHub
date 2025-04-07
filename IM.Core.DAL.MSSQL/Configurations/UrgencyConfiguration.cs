using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class UrgencyConfiguration : LookupConfiguration<Urgency>
    {
        protected override string TableName => "Urgency";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<Urgency> builder)
        {
            builder.Property(x => x.Sequence)
                .IsRequired()
                .HasColumnName("Sequence");

            builder.Property(x => x.RowVersion)
                .IsRequired();
        }
    }
}
