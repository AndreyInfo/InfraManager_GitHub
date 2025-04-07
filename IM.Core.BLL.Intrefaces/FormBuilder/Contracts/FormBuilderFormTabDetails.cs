using System;

namespace InfraManager.BLL.FormBuilder.Contracts
{
    public class FormBuilderFormTabDetails
    {
        public Guid ID { get; init; }
        public string Model { get; init; }
        public string Identifier { get; init; }
        public int Order { get; init; }
        public Guid FormID { get; init; }
        public string Type { get; init; }
        public string Icon { get; init; }
        public string Name { get; init; }
        public byte[] RowVersion { get; init; }
    }
}
