using System;

namespace InfraManager.DAL.Import.CSV
{
    public class CSVConfigurationTable
    {
        public Guid ID { get; init; }
        public string Name { get; set; }   
        public string Note { get; set; }
    }
}
