using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF.ServiceDesk;

[Route("bff/[controller]")]
[ApiController]
[Authorize]
public class ProblemDependenciesController : ControllerBase
{
    private readonly IProblemDependenciesBLL _problemDependencies;
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ProblemDependenciesController"/>.
    /// </summary>
    /// <param name="problemDependencies">BLL-зависимостей проблемы.</param>
    /// <param name="mapper">Маппер.</param>
    public ProblemDependenciesController(IProblemDependenciesBLL problemDependencies, IMapper mapper)
    {
        _problemDependencies = problemDependencies;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список зависимостей проблемы удовлетворяющих фильтру асинхронно.
    /// </summary>
    /// <param name="filterBy">Фильтр.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>
    /// Массив объектов <see cref="ProblemDependencyListItem"/>.
    /// Если объектов, удовлетворяющих фильтру не найдено, возвращает пустой массив.
    /// </returns>
    [HttpGet]
    public async Task<ProblemDependencyListItem[]> ListAsync([FromQuery] ProblemDependencyByProblemFilter filterBy, CancellationToken cancellationToken = default)
    {
        var problemDependencies = await _problemDependencies.GetDetailsArrayAsync(filterBy, cancellationToken);

        return _mapper.Map<ProblemDependencyListItem[]>(problemDependencies);
    }

    /// <summary>
    /// Добавить зависимость (связь) проблемы
    /// </summary>
    /// <param name="data">Данные для создания связи.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ProblemDependencyListItem> AddAsync([FromBody] ProblemDependencyDetailsModel data, CancellationToken cancellationToken = default)
    {
        var problemDependency = await _problemDependencies.AddAsync(data, cancellationToken);
        
        return _mapper.Map<ProblemDependencyListItem>(problemDependency);
    }

    /// <summary>
    /// Удалить зависимость проблемы асинхронно.
    /// </summary>
    /// <param name="id">Уникальный идентификатор объекта.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    [HttpDelete("{id}")]
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await _problemDependencies.DeleteAsync(id, cancellationToken);
    }
}