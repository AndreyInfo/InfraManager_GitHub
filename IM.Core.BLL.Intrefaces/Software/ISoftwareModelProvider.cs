using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.DAL.Software;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software;

/// <summary>
/// Интерфейс провайдера для модели ПО
/// </summary>
public interface ISoftwareModelProvider
{
    /// <summary>
    /// Возвращает необходимую реализацию объекта модели ПО в зависимости от шаблона
    /// </summary>
    /// <param name="model">Исходная модель ПО</param>
    /// <param name="modelTemplate">Шаблон модели</param>
    /// <returns></returns>
    public Task<SoftwareModelDetailsBase> GetAsync(SoftwareModel model, SoftwareModelTemplate modelTemplate);
}
