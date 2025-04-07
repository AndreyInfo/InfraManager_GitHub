using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ServiceDesk.Problems;

/// <summary>
/// Реализация интерфейса <see cref="IProblemDependenciesBLL"/> по-умолчанию.
/// Регистрируется в контейнере зависимостей самостоятельно.
/// </summary>
internal class ProblemDependenciesBLL : IProblemDependenciesBLL, ISelfRegisteredService<IProblemDependenciesBLL>
{
    private readonly ILogger<IProblemDependenciesBLL> _logger;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGetEntityArrayBLL<int, ProblemDependency, ProblemDependencyQueryResultItem, ProblemDependencyByProblemFilter> _arrayBLL;
    private readonly IInsertEntityBLL<ProblemDependency, ProblemDependencyDetailsModel> _insertEntityBLL;
    private readonly IRemoveEntityBLL<int, ProblemDependency> _removeEntityBLL;
    private readonly IBuildObject<ProblemDependencyQueryResultItem, ProblemDependency> _detailsBuilder;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ProblemDependenciesBLL"/>.
    /// </summary>
    /// <param name="currentUser">Текущий пользователь.</param>
    /// <param name="detailsBuilder">Создает <see cref="ProblemDependencyQueryResultItem"/> из <see cref="ProblemDependency"/>.</param>
    /// <param name="problemDependencyArrayBLL">Предоставляет доступ к данным в хранилище.</param>
    /// <param name="unitOfWork">Единица работы.</param>
    /// <param name="logger">Логер.</param>
    /// <param name="insertEntityBLL">Используется для вставки записи в хранилище.</param>
    /// <param name="removeEntityBLL">Используется для удаления записей в хранилище.</param>
    public ProblemDependenciesBLL(
        ICurrentUser currentUser,
        IBuildObject<ProblemDependencyQueryResultItem, ProblemDependency> detailsBuilder,
        IGetEntityArrayBLL<int, ProblemDependency, ProblemDependencyQueryResultItem, ProblemDependencyByProblemFilter> problemDependencyArrayBLL,
        IInsertEntityBLL<ProblemDependency, ProblemDependencyDetailsModel> insertEntityBLL,
        IUnitOfWork unitOfWork,
        ILogger<IProblemDependenciesBLL> logger,
        IRemoveEntityBLL<int, ProblemDependency> removeEntityBLL)
    {
        _currentUser = currentUser;
        _detailsBuilder = detailsBuilder;
        _arrayBLL = problemDependencyArrayBLL;
        _insertEntityBLL = insertEntityBLL;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _removeEntityBLL = removeEntityBLL;
    }

    public async Task<ProblemDependencyQueryResultItem[]> GetDetailsArrayAsync(ProblemDependencyByProblemFilter filterBy, CancellationToken cancellationToken)
    {
        return await _arrayBLL.ArrayAsync(filterBy, cancellationToken);
    }

    public async Task<ProblemDependencyQueryResultItem> AddAsync(ProblemDependencyDetailsModel data, CancellationToken cancellationToken = default)
    {
        var entity = await _insertEntityBLL.CreateAsync(data, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        _logger.LogInformation($"User (ID = {_currentUser.UserId}) inserted new {nameof(ProblemDependency)}.");

        return await _detailsBuilder.BuildAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _removeEntityBLL.RemoveAsync(id, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
        _logger.LogInformation($"User (ID = {_currentUser.UserId}) deleted {nameof(ProblemDependency)} (id = {id}).");
    }
}