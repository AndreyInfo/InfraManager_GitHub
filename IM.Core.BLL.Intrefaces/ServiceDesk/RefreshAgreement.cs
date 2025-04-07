using System;

namespace InfraManager.BLL.ServiceDesk;


public struct RefreshAgreement
{
    public DateTime? CountUtcCloseDateFrom { get; init; }
    public Guid? AgreementID { get; init; }
}