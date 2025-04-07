using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.FormBuilder
{
    public class FormField
    {
        public Guid ID { get; set; }
        public string Model { get; set; }
        public string Identifier { get; set; }
        public string CategoryName { get; set; } = "";
        public int Order { get; set; }
        public Guid TabID { get; set; }
        public FieldTypes Type { get; set; }
        public string SpecialFields { get; set; }
        public byte[] RowVersion { get; set; }
        public Guid? GroupFieldID { get; set; }
        public Guid? ColumnFieldID { get; set; }
        
        public virtual ICollection<FormField> Grouped { get; set; }
        public virtual ICollection<FormField> Columns { get; set; }

        public virtual ICollection<DynamicOptions> Options { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not FormField formField)
            {
                return false;
            }
            
            if (formField.Order != Order ||
                !formField.SpecialFields.Equals(SpecialFields) ||
                formField.Identifier != Identifier)
            {
                return false;
            }

            if (Grouped != null && formField.Grouped != null)
            {
                if (Grouped.Count != formField.Grouped.Count)
                {
                    return false;
                }

                foreach (var el in Grouped)
                {
                    var groupedField = formField.Grouped.FirstOrDefault(x => x.ID == el.ID);
                    
                    if (groupedField == null)
                    {
                        return false;
                    }

                    if (!el.Equals(groupedField))
                    {
                        return false;
                    }
                }
            }
            //TODO избавиться от этой копипасты
            if (Columns != null && formField.Columns != null)
            {
                if (Columns.Count != formField.Columns.Count)
                {
                    return false;
                }
                
                foreach (var el in Columns)
                {
                    var columnsField = formField.Columns.FirstOrDefault(x => x.ID == el.ID);
                    
                    if (columnsField == null)
                    {
                        return false;
                    }

                    if (!el.Equals(columnsField))
                    {
                        return false;
                    }
                }
            }
            
            if (formField.Options.Count != Options?.Count)
            {
                return false;
            }

            foreach (var option in Options)
            {
                var anotherOption = formField.Options.FirstOrDefault(x => x.ID == option.ID);
                if (anotherOption == null)
                {
                    return false;
                }

                if (!option.Equals(anotherOption))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
