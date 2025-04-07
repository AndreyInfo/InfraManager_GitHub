using System;

namespace IM.Core.DM.BLL.Interfaces.Models
{
    public class DBInfoModel
    {
        public Guid ID { get; set; }

        public string Version { get; set; }

        public bool IsPeripheral { get; set; }

        public bool IsCentral { get; set; }

        public bool SIDControl { get; set; }
    }
}
