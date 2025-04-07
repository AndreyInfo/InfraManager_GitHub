using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Import.CSV
{
    public class UICSVConfiguration
    {
        public UICSVConfiguration()
        {
            ID = Guid.NewGuid();
        }
        public Guid ID { get; init; }
        public string Name { get; set; }   
        public string Note { get; set; }
        public string Delimiter { get; set; }
        public byte[] RowVersion { get; init; }

    }
}
