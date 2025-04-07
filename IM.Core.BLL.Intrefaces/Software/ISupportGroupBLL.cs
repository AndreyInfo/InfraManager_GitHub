using InfraManager.BLL.Software.SoftwareModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software;
public interface ISupportGroupBLL
{
    /// <summary>
    /// Сохранение группы 1 линии поддержки для модели ПО.
    /// </summary>
    /// <param name="softwareModelID">Идентификатор модели ПО.</param>
    /// <param name="data">Данные модели ПО.</param>
    public void SaveSupportGroupForSoftwareModel(Guid softwareModelID, SoftwareModelData data);

    /// <summary>
    /// Обновление группы 1 линии поддержки для модели ПО.
    /// </summary>
    /// <param name="softwareModelID">Идентификатор модели ПО.</param>
    /// <param name="data">Данные модели ПО.</param>
    /// <returns></returns>
    public Task UpdateSupportGroupForSoftwareModelAsync(Guid softwareModelID, SoftwareModelData data, CancellationToken cancellationToken);
}
