using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class InfluenceConfiguration : LookupConfiguration<Influence>
    {
        protected override string TableName => "Influence";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<Influence> builder)
        {
            builder.Property(x => x.Sequence)
                .IsRequired()
                .HasColumnName("Sequence");

            builder.Property(x => x.RowVersion)
                .IsRequired();
        }
    }
}
