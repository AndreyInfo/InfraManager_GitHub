using System;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Sessions;

public class SessionFilter : BaseFilter
{
    public Guid? UserID { get; init; }
    public string UserAgent { get; init; }
}