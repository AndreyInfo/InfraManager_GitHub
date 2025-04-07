using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.CustomValues
{
    public class DefaultType : IGetValue
    {
        public ItemValue GetValue(string key, int order)
        => new ItemValue { Value = key, Order = order };
    }
}
