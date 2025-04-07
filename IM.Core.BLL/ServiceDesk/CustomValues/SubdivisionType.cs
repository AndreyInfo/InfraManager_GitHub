using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using System;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.CustomValues
{
    public class SubdivisionType : IGetValue
    {
        private readonly IRepository<Subdivision> _repository;
        public SubdivisionType(IRepository<Subdivision> repository)
        {
            _repository = repository;
        }

        public ItemValue GetValue(string key, int order)
        {
            if (Guid.TryParse(key, out Guid guid))
            {
                var subdivision = _repository
                    .FirstOrDefault(u => u.ID == guid);

                return new ItemValue { Value = subdivision.Name, ValueID = key, Order = order };
            }
            else
            {
                return new ItemValue();
            }
        }
    }


}
