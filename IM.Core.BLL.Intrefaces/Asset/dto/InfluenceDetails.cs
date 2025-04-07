using System;

namespace InfraManager.BLL.Asset.dto
{
    public class InfluenceDetails
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public byte[] RowVersion { get; set; }

        public int Sequence { get; set; }
    }
}
