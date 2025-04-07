using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;
public class ServiceFilter : BaseFilter
{
    public Guid? CategoryID { get; init; }
}
