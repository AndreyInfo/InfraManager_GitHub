using InfraManager.WebApi.Contracts.Models.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.CustomValues
{
    public interface IGetValue
    {
        ItemValue GetValue(string key, int order);
    }
}
