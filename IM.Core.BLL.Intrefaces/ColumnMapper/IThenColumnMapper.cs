using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.ColumnMapper;

/// <summary>
/// Класс хелпер для <see cref="IColumnMapperSettings"/> который позволяет создать маппинг на одно свойство сущности более
/// одного раза.
/// Используется в случае того, если при сортировки по одному полю нужно сортировать по более чем одному свойству из
/// сущности, например, при сортировки по полю версия, сначала нужно сортировать по MajorVersion, а потом по MinorVersion
/// </summary>
/// <typeparam name="TEntity">Сущность, свойства которой будут использоваться при сортировки</typeparam>
/// <typeparam name="TTable">Модель таблицы</typeparam>
public interface IThenColumnMapper<TEntity, TTable>
{
    IThenColumnMapper<TEntity, TTable> Then(Expression<Func<TEntity, object>> entityProperty);
}