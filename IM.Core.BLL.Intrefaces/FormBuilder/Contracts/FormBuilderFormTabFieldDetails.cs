using System;
using System.Collections.Generic;
using InfraManager.DAL.FormBuilder;

namespace InfraManager.BLL.FormBuilder.Contracts
{
    public class FormBuilderFormTabFieldDetails
    {
        public Guid ID { get; init; }
        public string Model { get; init; }
        public string Identifier { get; init; }
        public string CategoryName { get; init; } = "";
        public int Order { get; init; }
        public Guid TabID { get; init; }
        public FieldTypes Type { get; init; }
        public dynamic SpecialFields { get; init; }
        public string Icon { get; init; }
        public byte[] RowVersion { get; init; }
        
        public Guid? GroupFieldID { get; init; }
        public Guid? ColumnFieldID { get; init; }
        
        public IEnumerable<DynamicOptionsDetails> Options { get; init; }
        public IEnumerable<FormBuilderFormTabFieldDetails> Grouped { get; init; }
        public IEnumerable<FormBuilderFormTabFieldDetails> Columns { get; init; }
    }
}
