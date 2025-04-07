using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ChangeTracking
{
    internal class ChangeTrackingEntityEntryVisitor : IVisitEntityEntry, ISelfRegisteredService<IVisitEntityEntry>
    {
        #region .ctor

        private class EntryCopy
        {
            public EntityEntry Entry { get; init; }
            public IEntityState OriginalState { get; init; }
            public EntityState State { get; init; }
        }

        private readonly IServiceProvider _serviceProvider;
        private readonly List<EntryCopy> _entryCopies =
            new List<EntryCopy>();
        private readonly List<ITrackChanges> _selfTrackingChangesEntities =
            new List<ITrackChanges>();

        public ChangeTrackingEntityEntryVisitor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region реализация IVisitEntityEntry 

        public void Visit(EntityEntry entry)
        {
            VisitInternal(entry);
        }

        public void AfterSave()
        {
            foreach(var copy in _entryCopies)
            {
                VisitMethods[copy.State]
                    .MakeGenericMethod(copy.Entry.Metadata.ClrType)
                    .Invoke(this, new[] { copy.OriginalState, copy.Entry.Entity });
            }
            _entryCopies.Clear();
            SetSelfTrackingUnmodified();
        }

        private static Dictionary<EntityState, MethodInfo> VisitMethods =
            new Dictionary<EntityState, MethodInfo>
            {
                { 
                    EntityState.Added, 
                    typeof(ChangeTrackingEntityEntryVisitor)
                        .GetMethod(nameof(ChangeTrackingEntityEntryVisitor.VisitNewEntities)) 
                },
                {
                    EntityState.Modified,
                    typeof(ChangeTrackingEntityEntryVisitor)
                        .GetMethod(nameof(ChangeTrackingEntityEntryVisitor.VisitModifiedEntities))
                },
                {
                    EntityState.Deleted,
                    typeof(ChangeTrackingEntityEntryVisitor)
                        .GetMethod(nameof(ChangeTrackingEntityEntryVisitor.VisitDeletedEntities))
                }
            };

        private void VisitInternal(EntityEntry entry)
        {
            if (entry.Metadata.ClrType.IsAssignableTo(typeof(ITrackChanges)))
            {
                _selfTrackingChangesEntities.Add((ITrackChanges)entry.Entity);
            }
            else
            {
                _entryCopies.Add(
                    new EntryCopy
                    {
                        State = entry.State, // запоминаем State до сохранения
                        Entry = entry,
                        OriginalState = new PropertyValuesWrapper( // запоминаем OriginalState до сохранения
                            entry.OriginalValues.Clone(),
                            entry
                                .References
                                .ToDictionary(
                                    r => r.Metadata.Name,
                                    r => r.TargetEntry?.Entity))
                    });
            }
        }

        public void VisitDeletedEntities<T>(IEntityState originalState, T entity)where T : class
        {
            _serviceProvider
                .GetServices<IVisitDeletedEntity<T>>()
                .ForEach(visitor => visitor.Visit(originalState, entity));
        }

        public void VisitNewEntities<T>(IEntityState originalState, T entity)where T : class
        {
            _serviceProvider
                .GetServices<IVisitNewEntity<T>>()
                .ForEach(visitor => visitor.Visit(entity));
        }

        public void VisitModifiedEntities<T>(IEntityState originalState, T entity)where T : class
        {
            _serviceProvider
                .GetServices<IVisitModifiedEntity<T>>()
                .ForEach(visitor => visitor.Visit(originalState, entity));
        }

        private void SetSelfTrackingUnmodified()
        {
            foreach (var entity in _selfTrackingChangesEntities)
            {
                entity.SetUnmodified();
            }
            _selfTrackingChangesEntities.Clear();
        }

        #endregion

        #region реализация IVisitEntityEntry (асинхронная)

        public Task VisitAsync(EntityEntry entry, CancellationToken cancellationToken) // TODO: Не нужен больше async вариант
        {
            VisitInternal(entry);
            return Task.CompletedTask;
        }

        public async Task AfterSaveAsync(CancellationToken cancellationToken)
        {
            foreach (var copy in _entryCopies)
            {
                await (Task)VisitAsyncMethods[copy.State]
                    .MakeGenericMethod(copy.Entry.Metadata.ClrType)
                    .Invoke(this, new[] { copy.OriginalState, copy.Entry.Entity, cancellationToken });
            }
            _entryCopies.Clear();
            SetSelfTrackingUnmodified();
        }

        private static Dictionary<EntityState, MethodInfo> VisitAsyncMethods =
            new Dictionary<EntityState, MethodInfo>
            {
                { 
                    EntityState.Added, 
                    typeof(ChangeTrackingEntityEntryVisitor)
                        .GetMethod(nameof(ChangeTrackingEntityEntryVisitor.VisitNewEntitiesAsync)) 
                },
                {
                    EntityState.Modified,
                    typeof(ChangeTrackingEntityEntryVisitor)
                        .GetMethod(nameof(ChangeTrackingEntityEntryVisitor.VisitModifiedEntitiesAsync))
                },
                {
                    EntityState.Deleted,
                    typeof(ChangeTrackingEntityEntryVisitor)
                        .GetMethod(nameof(ChangeTrackingEntityEntryVisitor.VisitDeletedEntitiesAsync))
                }
            };

        public async Task VisitDeletedEntitiesAsync<T>(IEntityState originalState, T entity, CancellationToken cancellationToken) where T : class
        {
            var visitors = _serviceProvider.GetServices<IVisitDeletedEntity<T>>();
            foreach (var visitor in visitors)
            {
                await visitor.VisitAsync(originalState, entity, cancellationToken);
            }
        }

        public async Task VisitNewEntitiesAsync<T>(IEntityState originalState, T entity, CancellationToken cancellationToken) where T : class
        {
            var visitors = _serviceProvider.GetServices<IVisitNewEntity<T>>();
            foreach (var visitor in visitors)
            {
                await visitor.VisitAsync(entity, cancellationToken);
            }
        }

        public async Task VisitModifiedEntitiesAsync<T>(IEntityState originalState, T entity, CancellationToken cancellationToken) where T : class
        {
            var visitors = _serviceProvider.GetServices<IVisitModifiedEntity<T>>();
            foreach (var visitor in visitors)
            {
                await visitor.VisitAsync(originalState, entity, cancellationToken);
            }
        }

        #endregion     
    }
}
