using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace InfraManager.DAL.AssetsManagement.Hardware;

public interface IAssetQuery
{
    IQueryable<AssetListQueryResultItem> Query(Expression<Func<Asset.Asset, bool>> predicate = null);
    IQueryable<AssetListQueryResultItem> Query(IEnumerable<Expression<Func<Asset.Asset, bool>>> predicates);

    /// <summary>
    /// Получить список идентификаторов фильтром по элементам оргструктуры.
    /// </summary>
    /// <param name="itemID">Уникальный идентификатор элемента оргструктуры.</param>
    /// <param name="itemClass">Класс элемента оргструктуры.</param>
    /// <param name="field">Поле, по которому фильтровать.</param>
    /// <returns>Список идентификаторов.</returns>
    int[] QueryByOrgStructure(Guid itemID, ObjectClass itemClass, UserTreeSettings.FiltrationFieldEnum field);
}