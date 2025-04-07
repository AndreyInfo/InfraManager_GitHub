using System;

namespace InfraManager.BLL.Asset.dto
{
    public class InfrastructureSegmentDTO
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public byte[] RowVersion { get; set; }
    }
}
