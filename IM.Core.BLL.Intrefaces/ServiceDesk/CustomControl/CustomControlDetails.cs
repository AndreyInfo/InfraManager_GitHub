using System;

namespace InfraManager.BLL.ServiceDesk.CustomControl
{
    public class CustomControlDetails
    {
        public Guid UserID { get; init; }
        public Guid ObjectID { get; init; }
        public ObjectClass ClassID { get; init; }
        public bool UnderControl { get; init; }
    }
}
