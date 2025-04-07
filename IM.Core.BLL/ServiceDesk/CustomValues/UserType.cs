using InfraManager.DAL;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using System;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.CustomValues
{
    public class UserType : IGetValue
    {
        private readonly IRepository<User> _repository;
        public UserType(IRepository<User> repository)
        {
            _repository = repository;
        }

        public ItemValue GetValue(string key, int order)
        {
            if (Guid.TryParse(key, out Guid guid))
            {
                var user = _repository
                    .FirstOrDefault(u => u.IMObjID == guid);

                return new ItemValue { Value = user.FullName, ValueID = key, Order = order };
            }
            else
            {
                return new ItemValue();
            }
        }
    }


}
