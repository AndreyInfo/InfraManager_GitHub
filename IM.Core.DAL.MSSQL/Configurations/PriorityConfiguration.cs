using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class PriorityConfiguration : LookupConfiguration<Priority>
    {
        protected override string TableName => "Priority";

        protected override void ConfigureAdditionalMembers(EntityTypeBuilder<Priority> builder)
        {
            builder.Property(x => x.Color).HasColumnName("Color");
            builder.Property(x => x.Default).HasColumnName("Default");           
            builder.Property(x => x.Sequence).HasColumnName("Sequence");
            builder.Property(x => x.RowVersion).IsRowVersion().IsRequired();

            builder.IsMarkableForDelete();
        }
    }
}
