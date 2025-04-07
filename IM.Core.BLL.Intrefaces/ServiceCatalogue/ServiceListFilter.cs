using Inframanager.BLL;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Collections.Generic;

namespace InfraManager.BLL.ServiceCatalogue
{
    public class ServiceListFilter : ClientPageFilter<Service>
    {
        public ServiceListFilter()
        {
            OrderByProperty = nameof(Service.Name); // сортировка по умолчанию
        }

        public Guid? CategoryID { get; init; }
        public bool NullCategory { get; init; }
        public CatalogItemState[] StateList { get; init; }
    }
}
