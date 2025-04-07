using InfraManager.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq.Expressions;

namespace InfraManager.DAL
{
    internal static class DbContextExtensions
    {
        public static IEntityType Model<T>(this DbContext dbContext) where T : class
        {
            return dbContext.Model.FindEntityType(typeof(T));
        }

        public static StoreObjectIdentifier GetTableIdentifier(this IEntityType entityType) =>
            StoreObjectIdentifier.Table(entityType.GetTableName(), entityType.GetSchema());

        public static string ColumnName<TEntity, TProperty>(
            this DbContext dbContext,
            Expression<Func<TEntity, TProperty>> property) 
            where TEntity : class 
        {
            if (!property.TryGetPropertyName(out var propertyName))
            {
                throw new ArgumentException("Выражение не является свойством.", nameof(property));
            }
            var model = dbContext.Model<TEntity>();

            return dbContext.Model<TEntity>()
                .GetProperty(propertyName)
                .GetColumnName(model.GetTableIdentifier());
        }
    }
}
