using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace InfraManager.DAL
{
    public static class ModelBuilderExtensions
    {
        public static EntityTypeBuilder<T> IsMarkableForDelete<T>(this EntityTypeBuilder<T> builder)
            where T : class, IMarkableForDelete
        {
            builder.HasQueryFilter(x => !x.Removed);

            return builder;
        }

        public static OwnedNavigationBuilder<T, Description> WithDescription<T>(
            this EntityTypeBuilder<T> builder,
            Expression<Func<T, Description>> property) where T : class
        {
            var ownedPropertyBuilder = builder.OwnsOne(property);
            ownedPropertyBuilder.Property(x => x.Plain).HasMaxLength(1000);
            ownedPropertyBuilder.Property(x => x.Formatted).HasMaxLength(4000);
            ownedPropertyBuilder.Ignore(x => x.Original);

            return ownedPropertyBuilder;
        }
    }
}
