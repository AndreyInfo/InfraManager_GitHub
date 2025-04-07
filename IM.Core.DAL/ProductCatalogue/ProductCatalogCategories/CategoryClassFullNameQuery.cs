using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.ProductCatalogue.ProductCatalogCategories
{
    internal class CategoryClassFullNameQuery:ICategoryClassFullNameQuery,ISelfRegisteredService<ICategoryClassFullNameQuery>
    {
        private DbContext _context;

        public CategoryClassFullNameQuery(CrossPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<string> QueryAsync(Guid categoryId, CancellationToken token = default)
        {
            return await _context.Set<ProductCatalogCategory>().Where(x => x.ID == categoryId)
                .Select(x => DbFunctions.GetCategoryFullName(x.ID)).SingleOrDefaultAsync(token);
        }
    }
}