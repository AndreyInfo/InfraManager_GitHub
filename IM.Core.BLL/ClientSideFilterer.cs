using AutoMapper.Configuration.Conventions;
using InfraManager.BLL.ColumnMapper;
using InfraManager.BLL.Settings;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL;

/// <summary>
/// Аналог GuidePaggingFacade
/// Используется только в тех случаях, 
/// когда невозможно получить все данные для пагинации и фильтрации одним запросом к БД
/// Например получение всех листиков
/// Желательно не использовать
/// </summary>
internal class ClientSideFilterer<TDetails, TTable> : IClientSideFilterer<TDetails, TTable>
{
    private readonly IUserColumnSettingsBLL _columnBLL;
    private readonly ICurrentUser _currentUser;
    private readonly IColumnMapper<TDetails, TTable> _columnMapper;

    public ClientSideFilterer(IUserColumnSettingsBLL columnBLL
        , ICurrentUser currentUser
        , IColumnMapper<TDetails, TTable> columnMapper)
    {
        _columnBLL = columnBLL;
        _currentUser = currentUser;
        _columnMapper = columnMapper;
    }

    public async Task<TDetails[]> GetPaggingAsync(IEnumerable<TDetails> source,
            BaseFilter filter,
            Func<TDetails, bool> searchPredicate = null,
            CancellationToken cancellationToken = default)
    {
        if (searchPredicate is not null && !string.IsNullOrEmpty(filter.SearchString))
        {
            source = source.Where(searchPredicate);
        }

        var columns = await _columnBLL.GetAsync(_currentUser.UserId, filter.ViewName, cancellationToken);
        var orderColumn = columns.GetSortColumn();
        orderColumn.PropertyName = _columnMapper.MapFirst(orderColumn.PropertyName);

        var result = source.AsQueryable().OrderBy(orderColumn).AsEnumerable();
                                     
        if(filter.CountRecords != 0)
            result = result.Skip(filter.StartRecordIndex)
                           .Take(filter.CountRecords);

        return result.ToArray();
    }
}
