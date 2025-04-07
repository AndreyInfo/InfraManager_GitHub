using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Settings;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public class InframanagerObjectClass : IClassIM, ISelfRegisteredService<IClassIM>
    {
        private readonly IFinder<DAL.Settings.InframanagerObjectClass> _finder;
        private readonly IRepository<DAL.Settings.InframanagerObjectClass> _repository;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        public InframanagerObjectClass(IFinder<DAL.Settings.InframanagerObjectClass> finder,
                                       IMemoryCache memoryCache,
                                       IRepository<DAL.Settings.InframanagerObjectClass> repository,
                                       IMapper mapper)
        {
            _finder = finder;
            _memoryCache = memoryCache;
            _repository = repository;
            _mapper = mapper;
        }
        
        public Task<string> GetClassNameAsync(ObjectClass classID, CancellationToken cancellationToken = default)
        {
            return _memoryCache.GetOrCreateAsync($"ClassID_{classID}", async cache =>
            {
                var foundClass = await _finder.FindAsync(classID, cancellationToken);

                return foundClass == null ? string.Empty : foundClass.Name;
            });     
        }

        public string GetClassName(ObjectClass classID)
        {
            return _memoryCache.GetOrCreate($"ClassID_{classID}", cache =>
            {
                var foundClass = _finder.Find(classID);

                return foundClass == null ? string.Empty : foundClass.Name;
            });
            
        }

        private async Task<DAL.Settings.InframanagerObjectClass[]> GetInframanagerObjectsAsync(
            CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync("InframanagerObjects",
                entryKey => _repository.ToArrayAsync(cancellationToken));
        }
        

        public async Task<InframanagerObjectClassData[]> GetClassesByIDsAsync(List<ObjectClass> classIDs, CancellationToken cancellationToken = default)
        {
            var result = (await GetInframanagerObjectsAsync(cancellationToken)).Where(x => classIDs.Contains(x.ID));
            return _mapper.Map<InframanagerObjectClassData[]>(result);
        }
    }
}
