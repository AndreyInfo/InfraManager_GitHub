using InfraManager.BLL.Roles;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.Core.Extensions;

namespace IM.Core.HttpClient.UsersAndRoles
{
    public class UserRolesClient : ClientWithAuthorization
    {
        internal static string _url = "Roles/";
        public UserRolesClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<RoleDetails[]> GetRolesByUserAsync(Guid queryUserID, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<RoleDetails[]>($"{_url}by-user/{queryUserID}", userId, cancellationToken);
        }

        public async Task<RoleDetails> GetAsync(Guid roleID, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<RoleDetails>($"{_url}{roleID}", userId, cancellationToken);
        }

        public async Task<RoleDetails[]> GetRolesListAsync(object filter = null, Guid? userId = null, CancellationToken cancellationToken = default)
        {
            return await GetAsync<RoleDetails[], object>($"{_url}list/", filter, userId, cancellationToken);
        }
    }
}
