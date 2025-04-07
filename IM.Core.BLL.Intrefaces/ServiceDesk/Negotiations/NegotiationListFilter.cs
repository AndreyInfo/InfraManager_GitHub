using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public class NegotiationListFilter : ServiceDeskListFilter
    {
        public InframanagerObject? Parent { get; init; }
        public Guid? UserID { get; init; }
    }
}
