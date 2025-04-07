using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal abstract class LookupConfiguration<TLookup> : IEntityTypeConfiguration<TLookup>
        where TLookup : Lookup
    {
        public void Configure(EntityTypeBuilder<TLookup> builder)
        {
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(x => x.Name).HasMaxLength(250).HasColumnName("name").IsRequired();
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.ToTable(TableName);
            ConfigureAdditionalMembers(builder);
        }

        protected abstract string TableName { get; }

        protected abstract void ConfigureAdditionalMembers(EntityTypeBuilder<TLookup> builder);
    }
}