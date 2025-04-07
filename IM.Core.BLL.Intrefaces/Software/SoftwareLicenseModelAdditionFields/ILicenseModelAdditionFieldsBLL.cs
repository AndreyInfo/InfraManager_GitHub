using InfraManager.BLL.Software.SoftwareModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareLicenseModelAdditionFields;
public interface ILicenseModelAdditionFieldsBLL
{
    /// <summary>
    /// Сохранение доп. полей модели ПО: язык и схема лицензирования.
    /// </summary>
    /// <param name="softwareModelID">ID модели ПО.</param>
    /// <param name="data">Данные модели ПО.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task SaveAdditionalFieldsForSoftwareModelAsync(Guid softwareModelID, SoftwareModelData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление доп. полей модели ПО: язык и схема лицензирования.
    /// </summary>
    /// <param name="softwareModelID">ID модели ПО.</param>
    /// <param name="data">Данные модели ПО.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task UpdateAdditionalFieldsForSoftwareModelAsync(Guid softwareModelID, SoftwareModelData data, CancellationToken cancellationToken);
}
