using InfraManager.DAL.PGSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.Postgres
{
    internal static class EntityTypeBuilderExtensions
    {
        public static PropertyBuilder<byte[]> HasXminRowVersion<T>(
            this EntityTypeBuilder<T> builder,
            Expression<Func<T, byte[]>> rowVersion)
            where T : class
        {
            return builder
                .Property(rowVersion)
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .HasConversion<ulong>(
                    rowVersion => rowVersion.ConvertToPostgreXid(),
                    xid => xid.ConvertFromPostgreXid(),
                    new ValueComparer<byte[]>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c == null ? Array.Empty<byte>() : c.ToArray()))
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();// TODO: Это должно быть в общей конфигурации
        }
    }
}
