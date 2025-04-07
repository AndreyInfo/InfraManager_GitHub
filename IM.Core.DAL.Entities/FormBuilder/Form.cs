using InfraManager.DAL.FormBuilder.Enums;
using System;
using System.Collections.Generic;
using Inframanager;
using System.Linq;

namespace InfraManager.DAL.FormBuilder
{
    [ObjectClassMapping(ObjectClass.FormBuilder)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.FormBuilder_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.FormBuilder_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.FormBuilder_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.FormBuilder_Properties)]
    public class Form 
    {
        private const double C_lastIndex = 0.0001d;
        private const int C_initialMinorVersion = 1;
        public Form()
        {
            
        }
        
        public Guid ID { get; init; }
        public string Name { get; set; } 
        public string Identifier { get; set; } 
        public int Width { get; set; } = 500;
        public int Height { get; set; }
        public Guid ObjectID { get; set; }
        public int MinorVersion { get; set; } = C_initialMinorVersion;
        public ObjectClass ClassID { get; set; } 
        public int MajorVersion { get; set; }
        public string Description { get; set; } = ""; 
        public FormBuilderFormStatus Status { get; set; } = FormBuilderFormStatus.Created;
        public DateTime UtcChanged { get; set; } = DateTime.UtcNow;
        public bool FieldsIsRequired { get; set; }
        public double LastIndex { get; set; } = C_lastIndex;
        public Guid? ProductTypeID { get; set; }
        public Guid MainID { get; init; }

        public byte[] RowVersion { get; set; }
        
        public virtual ICollection<FormTab> FormTabs { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not Form form)
            {
                return false;
            }

            if (form.FormTabs.Count != FormTabs.Count)
            {
                return false;
            }

            foreach (var tab in FormTabs)
            {
                var anotherTab = form.FormTabs.FirstOrDefault(x => x.ID == tab.ID);
                if (anotherTab == null)
                {
                    return false;
                }

                if (!tab.Equals(anotherTab))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}
