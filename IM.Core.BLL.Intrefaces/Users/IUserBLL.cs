using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Users
{
    public interface IUserBLL
    {
        Task<UserListItem[]> ListAsync(UserListFilter filterBy, CancellationToken cancellationToken);
        Task<UserDetailsModel> DetailsAsync(Guid id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Костыль для поддержки функционала SystemSession
        /// TODO: Переделать / исправить функционал сессий, чтобы они могли работать без костыля и выпилить
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserDetailsModel> AnonymousDetailsAsync(Guid id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получение списка пользователей по фильтру
        /// </summary>
        /// <param name="filterBy">Фильтр для пользователей</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserDetailsModel[]> ListDetailsAsync(UserListFilter filterBy, CancellationToken cancellationToken = default);

        Task<UserDetailsModel> GetSignInUserAsync(string login, string password, bool needValidatePassword,
            CancellationToken cancellationToken = default);
        Task<UserListItem[]> FindByEmailAsync(string email, CancellationToken cancellationToken);
        /// <summary>
        /// Метод удаляет пользователя из базы данных 
        /// </summary>
        /// <param name="id">IMObjID</param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Метод обновляет данные о пользователе в базе данных
        /// </summary>
        Task UpdateAsync(Guid id, UserData userDetails, CancellationToken cancellationToken);
        /// <summary>
        /// Метод создает нового пользователя в базе данных
        /// </summary>
        Task CreateAsync(UserData userData, CancellationToken cancellationToken);

        /// <summary>
        /// Метод фильтрует пользователся по UserFilter
        /// (Опционально) фильтрует по subdivision или organization
        /// </summary>
        /// <param name="filter">фильтр для пользователей</param>
        [Obsolete("Доработать и использовать ListAsync")]
        Task<UserDetailsModel[]> GetTableAsync(UserFilter filter, CancellationToken cancellationToken);
        /// <summary>
        /// Возвращает список почты пользователей
        /// </summary>
        /// <param name="filter">Фильтр для выборки Email/param>
        /// <returns></returns>
        Task<UserEmailDetails[]> GetEmailsAsync(BaseFilter filter, CancellationToken cancellationToken);

        /// <summary>
        /// Получение данных о пользователе по его логину
        /// </summary>
        /// <param name="login"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserDetailsModel> DetailsByLoginAsync(string login, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение пользователя по его почте.
        /// При наличии нескольких пользователей выбирается первый, случайный.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UserDetailsModel> DetailsByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
