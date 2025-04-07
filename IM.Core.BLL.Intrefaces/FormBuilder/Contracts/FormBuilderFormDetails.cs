using System;

namespace InfraManager.BLL.FormBuilder.Contracts
{
    public class FormBuilderFormDetails
    {
        private const string defaultFormTypeValue = "Window";
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Guid ObjectID { get; set; }
        public string RowVersion { get; set; }
        public string Version => $"{MajorVersion}.{MinorVersion}";
        public string Class { get; set; }
        public ObjectClass ClassID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime UtcChanged { get; set; }
        public bool FieldsIsRequired { get; set; }
        public string Type { get; } = defaultFormTypeValue;
        public double LastIndex { get; set; }
        public Guid? ProductTypeID { get; set; }
        public int StatusCode { get; set; }
        public Guid MainID { get; init; }

        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
    }
}
