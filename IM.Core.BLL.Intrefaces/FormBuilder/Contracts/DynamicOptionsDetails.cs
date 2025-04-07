using System;
using InfraManager.DAL.FormBuilder.Enums;

namespace InfraManager.BLL.FormBuilder.Contracts
{
    public class DynamicOptionsDetails
    {
        public Guid ID { get; set; }
        public string Constant { get; set; } = string.Empty;
        public FormBuilderOperation OperationID { get; set; } = FormBuilderOperation.Equals;
        public FormBuilderAction ActionID { get; set; } = FormBuilderAction.MakeVisible;
        public string ParentIdentifier { get; set; } = string.Empty;
        public byte[] RowVersion { get; set; }
        public Guid WorkflowActivityFormFieldID { get; set; }
    }
}
