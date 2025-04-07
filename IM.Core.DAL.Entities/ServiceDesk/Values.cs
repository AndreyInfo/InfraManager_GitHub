using InfraManager.DAL.FormBuilder;
using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class Values
    {
        public long ID { get; set; }

        public long FormValuesID { get; set; }

        public Guid FormFieldID { get; set; }

        public virtual FormField FormField { get; set; }

        public string Value { get; set; }

        public byte[] RowVersion { get; set; }

        public int Order { get; set; }
        public int RowNumber { get; init; }
        public bool IsReadOnly { get; init; }
    }
}
