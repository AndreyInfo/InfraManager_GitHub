using System;

namespace InfraManager.BLL.Asset.dto
{
    public class PhoneTypeDTO
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public Guid? ComplementaryID { get; set; }

    }
}
