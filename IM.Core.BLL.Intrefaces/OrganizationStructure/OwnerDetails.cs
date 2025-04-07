using System;

namespace InfraManager.BLL.OrganizationStructure
{
    /// <summary>
    /// Этот класс представляет выходной контракт данных сущности "Владелец"
    /// </summary>
    public class OwnerDetails : OwnerData
    {
        public Guid IMObjID { get; init; }
    }
}
