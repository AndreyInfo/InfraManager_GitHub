using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.FormBuilder
{
    public class FormTab
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } = "Tab";
        public int Order { get; set; }
        public string Icon { get; set; }
        public string Model { get; set; }
        public string Identifier { get; set; }
        public Guid FormID { get; set; }
        public byte[] RowVersion { get; set; }
        
        public virtual ICollection<FormField> Fields { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not FormTab formTab)
            {
                return false;
            }

            if (formTab.Order != Order ||
                formTab.Name != Name ||
                formTab.Identifier != Identifier)
            {
                return false;
            }
            
            if (formTab.Fields.Count != Fields.Count)
            {
                return false;
            }

            foreach (var field in Fields)
            {
                var anotherField = formTab.Fields.FirstOrDefault(x => x.ID == field.ID);
                if (anotherField == null)
                {
                    return false;
                }

                if (!field.Equals(anotherField))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
