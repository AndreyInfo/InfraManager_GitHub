using InfraManager.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    internal class Finder<TEntity> : IFinder<TEntity> where TEntity : class
    {
        private readonly DbContext _db;
        private List<string> _includedProperties = new List<string>();

        public Finder(CrossPlatformDbContext db)
        {
            _db = db;
        }

        private DbSet<TEntity> Set() => _db.Set<TEntity>();

        private bool ShouldLoadNavigationProperties(TEntity entity) => entity != null && _includedProperties.Any();

        public TEntity Find(params object[] keys)
        {
            var entity = Set().Find(keys);
            
            if (ShouldLoadNavigationProperties(entity))
            {
                var entry = _db.Entry(entity);
                foreach(var property in _includedProperties)
                {
                    entry.Navigation(property).Load();
                }
            }

            _includedProperties.Clear();
            return entity;
        }

        public async ValueTask<TEntity> FindAsync(object[] keys, CancellationToken cancellationToken = default)
        {
            var entity = await Set().FindAsync(keys, cancellationToken);

            if (ShouldLoadNavigationProperties(entity))
            {
                var entry = _db.Entry(entity);
                foreach (var property in _includedProperties)
                {
                    await entry.Navigation(property).LoadAsync(cancellationToken);
                }
            }

            _includedProperties.Clear();
            return entity;
        }

        public IFinder<TEntity> With<TProperty>(Expression<Func<TEntity, TProperty>> includedProperty)
        {
            if (!includedProperty.TryGetPropertyName(out var propertyName))
            {
                throw new ArgumentException("Expression does not represent a property.", nameof(includedProperty));
            }

            _includedProperties.Add(propertyName);
            return this;
        }

        public IFinder<TEntity> WithMany<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> includedProperty)
        {
            if (!includedProperty.TryGetPropertyName(out var propertyName))
            {
                throw new ArgumentException("Expression does not represent a property.", nameof(includedProperty));
            }

            _includedProperties.Add(propertyName);
            return this;
        }
    }
}
