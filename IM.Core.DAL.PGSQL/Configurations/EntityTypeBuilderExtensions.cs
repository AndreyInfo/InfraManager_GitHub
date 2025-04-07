using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<T> WithRowVersion<T>(
            this EntityTypeBuilder<T> builder,
            Expression<Func<T, byte[]>> rowVersionAccessor) where T : class
        {
            builder.Property(rowVersionAccessor).HasColumnType("timestamp").IsConcurrencyToken();

            return builder;
        }
    }
}