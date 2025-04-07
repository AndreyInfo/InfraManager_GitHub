using System;

namespace InfraManager.DAL
{
    public class DataSourceInfo
    {
        public Guid OwnerID { get; set; }

        public Guid DataSourceID { get; set; }

        public string ProcessName { get; set; }

        public string MachineName { get; set; }

        public string IPAddresses { get; set; }

        public DateTime UtcCheckedAt { get; set; }
    }
}
