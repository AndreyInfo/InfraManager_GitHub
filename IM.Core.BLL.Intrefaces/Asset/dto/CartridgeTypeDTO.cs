using System;

namespace InfraManager.BLL.Asset.dto
{
    public class CartridgeTypeDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public Guid? ComplementaryID { get; set; }
    }
}
