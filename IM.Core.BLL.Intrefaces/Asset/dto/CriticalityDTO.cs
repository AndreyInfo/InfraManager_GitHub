using System;

namespace InfraManager.BLL.Asset.dto
{
    [Obsolete("Use LookupDetails<Guid>")]
    public class CriticalityDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
