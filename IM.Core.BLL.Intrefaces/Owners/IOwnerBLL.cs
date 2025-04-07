using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Owners;

[Obsolete("Use OrganizationStructure\\IOwnerBLL instead.")]
public interface IOwnerBLL
{
    /// <summary>
    /// Получение владельца по IMObjId
    /// </summary>
    /// <param name="id">IMObjId Владельца</param>
    public Task<OwnerDetails> GetAsync(Guid id,CancellationToken cancellationToken);
    /// <summary>
    /// Получение первого владельца из списка
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<OwnerDetails> GetFirstAsync(CancellationToken cancellationToken);
    /// <summary>
    /// Обновление владельца в базе
    /// </summary>
    /// <param name="ownerDetails">DTO Владельца</param>
    public Task UpdateAsync(OwnerDetails ownerDetails,CancellationToken cancellationToken);
    
}