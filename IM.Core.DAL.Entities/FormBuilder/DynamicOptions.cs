using InfraManager.DAL.FormBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.DAL.FormBuilder
{
    public class DynamicOptions
    {
        public Guid ID { get; set; }
        public string Constant { get; set; } = string.Empty;
        public FormBuilderOperation OperationID { get; set; } = FormBuilderOperation.Equals;
        public FormBuilderAction ActionID { get; set; } = FormBuilderAction.MakeVisible;
        public string ParentIdentifier { get; set; } = string.Empty;
        public Guid WorkflowActivityFormFieldID { get; set; }
        public byte[] RowVersion { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not DynamicOptions dynamicOptions)
            {
                return false;
            }

            return (dynamicOptions.Constant == Constant &&
                    dynamicOptions.OperationID == OperationID &&
                    dynamicOptions.ActionID == ActionID &&
                    dynamicOptions.ParentIdentifier == ParentIdentifier &&
                    dynamicOptions.WorkflowActivityFormFieldID == WorkflowActivityFormFieldID);
        }
    }
}
