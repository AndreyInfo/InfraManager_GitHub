using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    [Obsolete("Перестать использовать, т.к. не дает гибкости для настройки моделей")]
    internal abstract class LookupConfiguration<TLookup> : IEntityTypeConfiguration<TLookup>
        where TLookup : Lookup
    {
        public void Configure(EntityTypeBuilder<TLookup> builder)
        {
            builder.ToTable(TableName, "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .HasDefaultValueSql("NEWID()")
                .HasColumnName("ID");

            builder.Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired()
                .HasColumnName("Name");

            builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsRequired()
                .HasColumnType("timestamp")
                .HasColumnName("RowVersion");

            ConfigureAdditionalMembers(builder);
        }

        protected abstract string TableName { get; }

        protected abstract void ConfigureAdditionalMembers(EntityTypeBuilder<TLookup> builder);
    }
}
