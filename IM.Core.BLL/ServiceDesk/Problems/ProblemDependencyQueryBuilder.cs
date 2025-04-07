using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Problems;

/// <summary>
/// Представляет запрос к хранилищу сущностей <see cref="ProblemDependency"/> с фильтром <see cref="ProblemDependencyByProblemFilter"/>.
/// Регистрируется в контейнере зависимостей самомстоятельно. 
/// </summary>
internal class ProblemDependencyQueryBuilder :
    IBuildEntityQuery<ProblemDependency, ProblemDependencyQueryResultItem, ProblemDependencyByProblemFilter>,
    ISelfRegisteredService<IBuildEntityQuery<ProblemDependency, ProblemDependencyQueryResultItem, ProblemDependencyByProblemFilter>>
{
    private readonly IRepository<ProblemDependency> _repository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ProblemDependencyQueryBuilder"/>.
    /// </summary>
    /// <param name="repository">Хранилище сущностей <see cref="ProblemDependency"/>.</param>
    public ProblemDependencyQueryBuilder(IRepository<ProblemDependency> repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public IExecutableQuery<ProblemDependency> Query(ProblemDependencyByProblemFilter filterBy)
    {
        return _repository
            .Query()
            .Where(pd => pd.OwnerObjectID == filterBy.ProblemID);
    }
}