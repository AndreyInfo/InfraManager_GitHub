using InfraManager.BLL.Accounts.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Accounts;

public interface ITagBLL
{
    /// <summary>
    /// Получение тегов
    /// </summary>
    /// <param name="id">идентификатор объекта</param>
    /// <param name="classID">тип объекта</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модели тегов</returns>
    Task<TagDetailsModel> GetByUserAccountIDAsync(int ID, CancellationToken cancellationToken);

    ///// <summary>
    ///// Сохранение тегов
    ///// </summary>
    ///// <param name="saveModels">сохраняемые теги</param>
    ///// <param name="id">идентификатор объекта</param>
    ///// <param name="classID">тип объекта</param>
    ///// <param name="cancellationToken"></param>
    Task UpdateAsync(TagDataModel tagData, CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Удаляет тег
    ///// </summary>
    ///// <param name="tag">Объект Тег</param>
    ///// <param name="cancellationToken"></param>
    ///// <returns></returns>
    Task DeleteAsync(TagData tag, CancellationToken cancellationToken = default);

    ///// <summary>
    ///// Создаёт тег
    ///// </summary>
    ///// <param name="tag">Объект Тег</param>
    ///// <param name="cancellationToken"></param>
    ///// <returns></returns>
    Task<TagDetails> CreateAsync(TagData tag, CancellationToken cancellationToken = default);
}
