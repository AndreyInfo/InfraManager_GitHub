using Inframanager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL;
using InfraManager.DAL.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Accounts
{
    public class UserAccountQueryBuilder : IBuildEntityQuery<UserAccount, UserAccountDetails, UserAccountFilter>, ISelfRegisteredService<IBuildEntityQuery<UserAccount, UserAccountDetails, UserAccountFilter>>
    {
        private readonly IReadonlyRepository<UserAccount> _reposiory;
        
        public UserAccountQueryBuilder(IReadonlyRepository<UserAccount> reposiory)
        {
            _reposiory = reposiory;
        }

        public IExecutableQuery<UserAccount> Query(UserAccountFilter filterBy)
        {
            var query = _reposiory.Query();

            if(!string.IsNullOrEmpty(filterBy.SearchString))
            {
                query = query.Where(ua => ua.Name.ToLower().Contains(filterBy.SearchString.ToLower()) || 
                ua.Login.ToLower().Contains(filterBy.SearchString.ToLower()));
            }

            return query;
        }
    }
}
