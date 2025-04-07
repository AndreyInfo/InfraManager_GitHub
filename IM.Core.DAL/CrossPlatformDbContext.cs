using System;
using Inframanager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InfraManager.DAL
{
    /// <summary>
    /// Этот класс представляет универсальный контекст данных
    /// </summary>
    internal class CrossPlatformDbContext : DbContext, IUnitOfWork
    {
        #region .ctor

        /// <summary>
        /// Инициализирует свойства абстрактного контекста данных
        /// </summary>
        /// <param name="dbConfigurer">Конфигуратор контекста данных для работы с конкретной СуБД</param>
        public CrossPlatformDbContext(
            IConfigureDbContext dbConfigurer,
            IBuildDbContextModel dbModelBuilder,
            IEnumerable<IVisitEntityEntry> entryVisitors)
        {
            _dbConfigurer = dbConfigurer;
            _dbModelBuilder = dbModelBuilder;
            _entryVisitors = entryVisitors;
        }

        #endregion

        #region configuration

        private readonly IConfigureDbContext _dbConfigurer;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _dbConfigurer.Configure(optionsBuilder);
        }

        #endregion

        #region model building

        private readonly IBuildDbContextModel _dbModelBuilder;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_dbConfigurer.Schema);
            _dbModelBuilder.BuildModel(modelBuilder);
        }

        #endregion

        #region ISaveChangesCommand

        private readonly IEnumerable<IVisitEntityEntry> _entryVisitors;

        /// <inheritdoc/>
        public void Save(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var transaction =
                   new TransactionScope(
                       TransactionScopeOption.Required,
                       new TransactionOptions { IsolationLevel = isolationLevel }))
            {
                do
                {
                    VisitAllEntries();

                    try
                    {
                        SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw new ConcurrencyException();
                    }
                    catch (DbUpdateException error)
                    {
                        HandleException(error);
                    }

                    _entryVisitors.ForEach(v => v.AfterSave());
                }
                while (HasChanges());

                transaction.Complete();
            }
        }

        /// <inheritdoc/>
        public async Task SaveAsync(CancellationToken cancellationToken = default,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using (var transaction =
                   new TransactionScope(
                       TransactionScopeOption.Required,
                       new TransactionOptions { IsolationLevel = isolationLevel },
                       TransactionScopeAsyncFlowOption.Enabled))
            {
                do
                {
                    await VisitAllEntriesAsync(cancellationToken);

                    try
                    {
                        await SaveChangesAsync(cancellationToken);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        throw new ConcurrencyException();
                    }
                    catch (DbUpdateException error)
                    {
                        HandleException(error);

                        // Если данный тип не сконфигурирован на обработку ошибки, то вернем базовую
                        throw;
                    }

                    foreach (var visitor in _entryVisitors)
                    {
                        await visitor.AfterSaveAsync(cancellationToken);
                    }                
                }
                while (HasChanges());

                transaction.Complete();
            }
        }
        
        private static bool FindModifiedRecursive(NavigationEntry memberEntry)
        {           
            return 
                memberEntry.IsModified
                || (memberEntry.IsLoaded
                    && memberEntry is ReferenceEntry referenceEntry
                    && (referenceEntry.TargetEntry?.Navigations.Any(FindModifiedRecursive) ?? true));
        }

        private void VisitAllEntries()
        {
            foreach (var entry in GetChanges())
            {
                _entryVisitors.ForEach(v => v.Visit(entry));
            }
        }

        private async Task VisitAllEntriesAsync(CancellationToken cancellationToken)
        {
            foreach (var entry in GetChanges())
            {
                foreach (var visitor in _entryVisitors)
                {
                    await visitor.VisitAsync(entry, cancellationToken);
                }
            }
        }

        private void HandleException(DbUpdateException error)
        {
            foreach (var entry in error.Entries)
            {
                var uniqueKey = entry.Metadata.GetIndexes()
                    .FirstOrDefault(
                        ix => ix.IsUnique
                              && (error.Message.Contains(ix.GetDatabaseName())
                                  || (error.InnerException?.Message?.Contains(ix.GetDatabaseName()) ?? false)));
                if (uniqueKey != null)
                {
                    throw new UniqueKeyConstraintViolationException(
                        ToCsString(uniqueKey.Properties),
                        entry.Metadata.ClrType,
                        error);
                }

                var foreignAndReferencingFK = entry.Metadata.GetReferencingForeignKeys().ToList();
                foreignAndReferencingFK.AddRange(entry.Metadata.GetForeignKeys());

                var foreignKey = foreignAndReferencingFK.FirstOrDefault(
                    fk => error.Message.Contains(fk.GetConstraintName())
                          || (error.InnerException?.Message.Contains(fk.GetConstraintName()) ?? false));

                if (foreignKey != null)
                {
                    throw new ForeignKeyConstraintViolationException(
                        ToCsString(foreignKey.Properties),
                        entry.Metadata.ClrType,
                        error);
                }
            }
        }

        private string ToCsString(IReadOnlyList<IProperty> properties)
        {
            return string.Join(", ", properties.Select(p => p.Name));
        }

        private static EntityState[] ModifiedStates = new[] { EntityState.Modified, EntityState.Added, EntityState.Deleted };
        private IEnumerable<EntityEntry> GetChanges()
        {
            return ChangeTracker.Entries().Where(entry => ModifiedStates.Contains(entry.State));
        }
        public bool HasChanges() => GetChanges().Any();

        #endregion
    }
}