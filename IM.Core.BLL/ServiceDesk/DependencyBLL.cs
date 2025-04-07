using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    // TODO: Переписать (несколько человек занимались инфраструктурой и каждый запилил свой сервис)
    public class DependencyBLL<TDependency> : IDependencyBLL<TDependency>
        where TDependency : Dependency
    {
        private readonly ILogger<DependencyBLL<TDependency>> _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IReadonlyRepository<TDependency> _repository;
        private readonly IMapper _mapper;

        public DependencyBLL(
            ILogger<DependencyBLL<TDependency>> logger,
            ICurrentUser currentUser,
            IReadonlyRepository<TDependency> repository,
            IMapper mapper)
        {
            _logger = logger;
            _currentUser = currentUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DependencyDetails[]> GetDependenciesAsync(Guid objectID, CancellationToken cancellationToken)
        {
            _logger.LogTrace(
                $"User (ID = {_currentUser.UserId}) requested Dependencies of {typeof(TDependency).Name} for object {objectID}.");
            var dependencies = await _repository.Query().Where(x=>x.OwnerObjectID == objectID).ExecuteAsync(cancellationToken);
            _logger.LogTrace(
                $"{dependencies.Count()} dependencies of type {typeof(TDependency).Name} loaded for  object {objectID} (user ID = {_currentUser.UserId})");

            return _mapper.Map<DependencyDetails[]>(dependencies);
        }
   }
}