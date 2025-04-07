using System;

namespace InfraManager.BLL.Asset
{
    public class RemoveModel
    {
        public Guid ID { get; }

        public string Name { get; }

        public byte[] RowVersion { get; }
    }

    public class RemoveModeUniversal<TypeId> // TypeId - Guid || int
    {
        public TypeId ID { get; }

        public string Name { get; }

        public byte[] RowVersion { get; }
    }
}
