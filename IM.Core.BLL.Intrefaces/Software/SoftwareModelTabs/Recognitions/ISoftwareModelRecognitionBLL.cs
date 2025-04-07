using InfraManager.BLL.Software.SoftwareModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Recognitions;
public interface ISoftwareModelRecognitionBLL
{
    /// <summary>
    /// Сохранение распознавания для модели ПО.
    /// </summary>
    /// <param name="softwareModelID">ID модели ПО.</param>
    /// <param name="data">Данные модели ПО.</param>
    public void SaveModelRecognitionForSoftwareModel(Guid softwareModelID, SoftwareModelData data);

    /// <summary>
    /// Обновление распознавания для модели ПО.
    /// </summary>
    /// <param name="softwareModelID">ID модели ПО.</param>
    /// <param name="data">Данные модели ПО.</param>
    /// <returns></returns>
    public Task UpdateModelRecognitionForSoftwareModelAsync(Guid softwareModelID, SoftwareModelData data, CancellationToken cancellationToken);

}
