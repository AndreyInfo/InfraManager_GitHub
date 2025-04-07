using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.WebAPIClient
{
    public class UsersClient : ClientWithAuthorizationBase
    {
        internal static string _url = "Users/";
        public UsersClient(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<bool> ExistsByEmail(string email, Guid? userId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var users = await GetAsync<UserListItemModel[]>($"{_url}by-email/{email}", userId, cancellationToken);
            return users?.Any() ?? false;
        }

        public class UserListItemModel
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
        }
    }
}
