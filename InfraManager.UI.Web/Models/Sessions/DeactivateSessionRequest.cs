using System;

namespace InfraManager.UI.Web.Models.Sessions;

public class DeactivateSessionRequest
{
    public Guid UserID { get; init; }
    public string UserAgent { get; init; }
}