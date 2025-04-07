using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class PriorityConfiguration : LookupConfiguration<Priority>
    {
        protected override string TableName => "priority";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<Priority> builder)
        {
            builder.Property(x => x.Color).HasColumnName("color").IsRequired();
            builder.Property(x => x.Default).HasColumnName("is_default").IsRequired();
            builder.Property(x => x.Removed).HasColumnName("removed").IsRequired();
            builder.Property(x => x.Sequence).HasColumnName("sequence").IsRequired();

            builder.IsMarkableForDelete();

            builder.HasXminRowVersion(e => e.RowVersion);
        }
    }
}