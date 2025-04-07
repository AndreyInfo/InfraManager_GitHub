using Inframanager;
using System;

namespace InfraManager.DAL.AccessManagement
{
    /// <summary>
    /// Этот интерфейс описывает построителя спецификации, завязанной на вызове func_accessIsGranted
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    public interface IBuildAccessIsGrantedSpecification<T> : 
        IBuildSpecification<T, User>,
        IBuildSpecification<T, Guid>
        where T : IGloballyIdentifiedEntity
    {
    }
}
