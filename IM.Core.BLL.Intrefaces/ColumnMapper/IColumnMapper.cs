using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.ColumnMapper;
/// <summary>
/// Позволяет мапить колонки из таблицы в свойства из сущностей
/// Например, колонка Версия в таблице состоит из двух свойств(MajorVersion, MinorVersion), при сортировке на фронте по
/// данной колонке будет непонятно по какому критерию производить сортировку в бд, но с помощью этого класса мы можем
/// подсказать построителю запроса по каким свойствам сущности нужно производить сортировку
/// </summary>
/// <typeparam name="TEntity">Сущность, свойства которой будут использоваться при сортировки</typeparam>
/// <typeparam name="TTable">Модель таблицы</typeparam>
public interface IColumnMapper<TEntity, TTable>
{
    Expression<Func<TEntity, object>>[] MapToRightColumn(string tablePropertyName);
}