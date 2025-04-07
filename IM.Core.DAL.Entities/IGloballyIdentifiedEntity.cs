using System;

namespace InfraManager.DAL
{
    /// <summary>
    /// Этот интерфейс описывает сущности, имеющие глобальный идентификатор
    /// </summary>
    public interface IGloballyIdentifiedEntity
    {
        /// <summary>
        /// Возвращает глобальный идентификатор сущности
        /// </summary>
        Guid IMObjID { get; }
    }
}
