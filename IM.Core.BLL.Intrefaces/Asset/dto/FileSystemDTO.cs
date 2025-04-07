using System;

namespace InfraManager.BLL.Asset.dto
{
    public class FileSystemDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public byte[] RowVersion { get; set; }
        public Guid? ComplementaryID { get; set; }
    }
}
