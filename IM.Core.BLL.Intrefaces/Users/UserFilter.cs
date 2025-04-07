using System;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Users;

public class UserFilter : BaseFilter
{
    public ObjectClass ClassID { get; init; }
    public Guid ID { get; init; }
    public bool MateriallyResponsibleOnly { get; init; }
    public bool ExecutorOnly { get; init; }
    public bool OwnerOnly { get; init; }
    public bool IsSupportAdministrator { get; init; }
    public bool OnlyParticipantsAgreement { get; init; }
    public bool? WithEmails { get; init; }
    
    /// <summary>
    /// Может ли пользователь быть экспертом БЗ
    /// </summary>
    public bool OnlyKBExpert { get; init; }
}