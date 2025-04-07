using System;

namespace InfraManager.DAL.ServiceCatalogue
{
    public class ServiceCenter : Catalog<Guid>
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Manager { get; set; }
        public string Notice { get; set; }
        public Guid? PeripheralDatabaseID { get; set; }
        public Guid? ComplementaryID { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
