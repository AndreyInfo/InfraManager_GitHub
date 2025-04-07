using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using System;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.CustomValues
{
    public class PositionType : IGetValue
    {
        private readonly IRepository<JobTitle> _repository;
        public PositionType(IRepository<JobTitle> repository)
        {
            _repository = repository;
        }


        public ItemValue GetValue(string key, int order)
        {
            if (Guid.TryParse(key, out Guid guid))
            {
                var user = _repository
                    .FirstOrDefault(u => u.IMObjID == guid);

                return new ItemValue { Value = user.Name, ValueID = key, Order = order };
            }
            else
            {
                return new ItemValue();
            }
        }
    }


}
