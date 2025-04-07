using System;

namespace InfraManager.DAL.Import
{
    public class ImportTypeConfigurationBase
    {
        public Guid ID { get; init; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public byte[]? RowVersion { get; set; }
    }
}
