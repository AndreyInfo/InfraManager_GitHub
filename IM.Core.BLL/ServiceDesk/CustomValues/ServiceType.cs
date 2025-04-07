using System;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.CustomValues;

internal class ServiceType : IGetValue
{
    private readonly IFinder<Service> _finder;

    public ServiceType(IFinder<Service> finder)
    {
        _finder = finder;
    }

    public ItemValue GetValue(string key, int order)
    {
        if (Guid.TryParse(key, out var id))
        {
            var service = _finder.Find(id);
            if (service != null)
            {
                return new ItemValue
                {
                    Value = service.Name,
                    ValueID = key,
                    Order = order,
                };
            }
        }

        return new ItemValue();
    }
}