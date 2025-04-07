using Inframanager;
using Inframanager.BLL;
using InfraManager.DAL.OrganizationStructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.OrganizationStructure
{
    // TODO: Убрать этот класс
    // 0) Нарушен принцип Single Responsibility
    // 1) IValidateObjectPermissions<Guid, DeputyUser> для типа, который не реализует интерфейс IGloballyIdentifiedEntity, итак региструется сервис с идентичным поведением
    // 2) Не может такого быть, чтобы для работы с заместителями не требовалось операций для IValidatePermissions<DeputyUser>. Это дыра в безопасности
    internal class DeputyUserPermissions : 
        IValidateObjectPermissions<Guid, DeputyUser>,
        ISelfRegisteredService<IValidateObjectPermissions<Guid, DeputyUser>>,
        IValidatePermissions<DeputyUser>,
        ISelfRegisteredService<IValidatePermissions<DeputyUser>>
    {
        public IEnumerable<Expression<Func<DeputyUser, bool>>> ObjectIsAvailable(Guid userId)
        {
            return Array.Empty<Expression<Func<DeputyUser, bool>>>();
        }

        public Task<bool> ObjectIsAvailableAsync(Guid userId, Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }

        public Task<bool> UserHasPermissionAsync(Guid userId, ObjectAction action, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }
}
