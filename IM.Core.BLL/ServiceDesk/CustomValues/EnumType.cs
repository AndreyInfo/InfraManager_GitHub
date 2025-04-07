using InfraManager.DAL;
using InfraManager.DAL.Parameters;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using System;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.CustomValues
{

    public class EnumType : IGetValue
    {
        private readonly IRepository<ParameterEnumValue> _repository;
        public EnumType(IRepository<ParameterEnumValue> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public ItemValue GetValue(string key, int order)
        {

            if (Guid.TryParse(key, out Guid id))
            {
                var parameterEnumValue = _repository
                    .FirstOrDefault(u => u.ID == id);

                return new ItemValue { Value = parameterEnumValue.Value, ValueID = key, Order = order };
            }
            else
            {
                return new ItemValue();
            }
        }

    }


}
