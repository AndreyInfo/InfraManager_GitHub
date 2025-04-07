using Inframanager;
using System;

namespace InfraManager.DAL.Finance
{
    [ObjectClassMapping(ObjectClass.Supplier)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class Supplier
    {
        public Guid ID { get; init; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Notice { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }
        public Guid? ComplementaryID { get; set; }
        public string RegisteredAddress { get; set; }
        public string ExternalID { get; set; }
    }
}
