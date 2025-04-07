using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Users
{
    /// <summary>
    /// Интрефем для получения даных о системном пользователе
    /// </summary>
    public interface ISystemUserGetter
    {
        /// <summary>
        /// Получение системного пользователя асинхронно
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<User> GetAsync(CancellationToken cancellationToken = default);
    }
}