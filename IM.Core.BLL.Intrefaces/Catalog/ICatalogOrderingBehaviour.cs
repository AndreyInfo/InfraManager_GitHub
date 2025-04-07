using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Catalog
{
    public interface ICatalogOrderingBehaviour<TCatalog> where TCatalog : class
    {
        IQueryable<TCatalog> GetCatalogOrderedQuery(IQueryable<TCatalog> catalogs, BaseFilter filter);
    }
}
