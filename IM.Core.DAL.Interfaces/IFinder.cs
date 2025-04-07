using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    /// <summary>
    /// Этот интерфейс описывает универсальный поиск сущности по ключевым значениям
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public interface IFinder<TEntity>
    {
        TEntity Find(params object[] keys);
        ValueTask<TEntity> FindAsync(object[] keys, CancellationToken token = default);
        IFinder<TEntity> With<TProperty>(Expression<Func<TEntity, TProperty>> include);
        IFinder<TEntity> WithMany<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> include);
    }
}
