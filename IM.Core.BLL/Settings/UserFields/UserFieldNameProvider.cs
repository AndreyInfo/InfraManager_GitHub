using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Settings.UserFields;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings.UserFields
{
    internal class UserFieldNameBLL<T> : IUserFieldNameBLL where T : class, IUserFieldName
    {
        private const string defaultFieldName = "Пользовательское поле";

        private readonly IReadonlyRepository<T> _repository;
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPagingQueryCreator _paggingQuery;

        private static string CacheKey => $"UserFieldName_{typeof(T)}";
        public UserFieldNameBLL(IReadonlyRepository<T> repository
            , IMemoryCache memoryCache
            , IUnitOfWork unitOfWork
            , IMapper mapper
            , IPagingQueryCreator paggingQuery)
        {     
            _repository = repository;
            _memoryCache = memoryCache;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paggingQuery = paggingQuery;
        }

        public string GetName(FieldNumber number)
        {
            var fields = _memoryCache.GetOrCreate(
                CacheKey,
                entry => _repository.ToDictionary(x => x.ID, x => x.Name));

            return fields[number];
        }

        public async Task<string> GetNameAsync(FieldNumber number, CancellationToken cancellationToken = default)
        {
            var fields = await GetFromCacheAsync(cancellationToken);

            return fields[number];
        }

        public async Task<bool> GetVisibilityAsync(FieldNumber number, CancellationToken cancellationToken = default)
        {
            var fields = await GetFromCacheAsync(cancellationToken);
            return !IsDefault(number, fields[number]);
        }

        private static bool IsDefault(FieldNumber number, string name)
        {
            return name == $"{defaultFieldName} {(byte)number}";
        }

        public async Task<KeyValuePair<FieldNumber, string>[]> ListVisibleAsync(CancellationToken cancellationToken = default)
        {
            var userFields = await GetFromCacheAsync(cancellationToken);

            return userFields.ToArray();
        }

        public async Task<KeyValuePair<FieldNumber, string>[]> ListNonDefaultAsync(CancellationToken cancellationToken = default)
        {
            var userFields = await GetFromCacheAsync(cancellationToken);
            return userFields.Where(f => !IsDefault(f.Key, f.Value)).ToArray();            
        }


        private Task<Dictionary<FieldNumber, string>> GetFromCacheAsync(CancellationToken cancellationToken = default)
        {
            return _memoryCache.GetOrCreateAsync(
                CacheKey,
                entry => GetFromRepositoryAsync(cancellationToken));
        }

        private async Task<Dictionary<FieldNumber, string>> GetFromRepositoryAsync(CancellationToken cancellationToken)
        {
            var pagging = _paggingQuery.Create(_repository.Query().OrderBy(c => c.ID));
            var entities = await pagging.PageAsync(0, 0, cancellationToken);
            
            var result = new Dictionary<FieldNumber, string>();
            foreach(var userField in entities)
            {
                result.TryAdd(userField.ID, userField.Name);
            }
            return result;
        }

        public async Task<FieldNumber> UpdateAsync(FieldNumber number, UserFieldData model, CancellationToken cancellationToken)
        {
            var field = await _repository.FirstOrDefaultAsync(c => c.ID == number, cancellationToken)
                ?? throw new ObjectNotFoundException($"Not found userfield `{number}` for {typeof(T).Name}");

            _mapper.Map(model, field);
            await _unitOfWork.SaveAsync(cancellationToken);
            RemoveCache();

            return number;
        }

        private void RemoveCache()
        {
            _memoryCache.Remove(CacheKey);
        }
    }
}
