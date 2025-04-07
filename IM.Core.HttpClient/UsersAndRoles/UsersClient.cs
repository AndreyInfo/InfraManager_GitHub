using InfraManager.BLL;
using InfraManager.BLL.Users;
using InfraManager.DAL.Users;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IM.Core.HttpClient.UsersAndRoles
{
    public class UsersClient : ClientWithAuthorization
    {
        internal static string _url = "Users/";
        public UsersClient(string baseUrl) : base(baseUrl)
        {
        }

        //  TODO: Раскоментировать и поправить модели по готовности WebAPI
        public async Task<UserDetailsModel> GetAsync(Guid guid, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<UserDetailsModel>($"{_url}{guid}", userId, cancellationToken);
        }

        public async Task<UserDetailsModel> GetByEmailAsync(string email, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<UserDetailsModel>($"{_url}single-by-email/{email}", userId, cancellationToken);
        }

        public async Task<UserDetailsModel> GetByLoginAsync(string login, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            login = login.Replace("\\", "%5C");
            return await GetAsync<UserDetailsModel>($"{_url}by-login/{login}", userId, cancellationToken);
        }

        //public async Task<UserDetailsModel> SaveAsync(UserModel User, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await SaveAsync<UserDetailsModel, UserModel>(_url, User, userId, cancellationToken);
        //}

        //public async Task<UserDetailsModel> AddAsync(UserModel User, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    return await AddAsync<UserDetailsModel, UserModel>(_url, User, userId, cancellationToken);
        //}
        public async Task<UserListItem[]> GetByEmail(string email, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<UserListItem[]>($"{_url}by-email/{email}", userId, cancellationToken);
        }
        public async Task<UserListItem[]> GetList(UserListFilter filter = null, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<UserListItem[], UserListFilter>(_url, filter, userId, cancellationToken);
        }
        public async Task<UserDetailsModel[]> ListDetailsAsync(UserListFilter listFilter = null, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetListByPostAsync<UserDetailsModel[], UserListFilter>($"{_url}ListDetailsAsync", listFilter, userId, cancellationToken);
        }
    }
}
