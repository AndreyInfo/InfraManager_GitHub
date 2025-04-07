using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Asset
{
    internal class ProcessorsListQuery : IProcessorsListQuery, ISelfRegisteredService<IProcessorsListQuery>
    {
        private readonly DbContext _db;

        public ProcessorsListQuery(CrossPlatformDbContext db)
        {
            _db = db;
        }

        public async Task<List<ProcessorsQueryResponseItem>> QueryAsync(ProcessorsQueryRequest request, CancellationToken token = default)
        {
            var productCatalogTypes = _db.Set<ProductCatalogType>().AsNoTracking();
            if (request.TemplateID.HasValue)
            {
                productCatalogTypes = productCatalogTypes.Where(x => x.ProductCatalogTemplateID == request.TemplateID.Value);
            }

            var query = _db.Set<AdapterType>()
                .Include(at => at.Vendor)
                .Include(at => at.ProductCatalogType)
                .Join(productCatalogTypes, at => at.ProductCatalogTypeID, pc => pc.IMObjID, (at, pc) => at)
                .SelectMany(
                    at => _db.Set<Processor>()
                        .Where(pr => pr.ID == at.IMObjID)
                        .DefaultIfEmpty(), 
                    (at, pr) => new { AdapterType = at, Processor = pr });

            if (request.TypeID != null)
            {
                query = query.Where(x => x.AdapterType.ProductCatalogTypeID == request.TypeID.Value);
            }

            if (request.CategoryID != null)
            {
                query = query.Where(x => x.AdapterType.ProductCatalogType.ProductCatalogCategoryID == request.CategoryID.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.NameSearch))
            {
                query = query.Where(x => x.AdapterType.Name.Contains(request.NameSearch));
            }

            var result = await query.ToListAsync(token);

            return result
                .Select(x => new ProcessorsQueryResponseItem(x.AdapterType, x.AdapterType.Vendor, x.Processor)).ToList();
        }
    }
}
