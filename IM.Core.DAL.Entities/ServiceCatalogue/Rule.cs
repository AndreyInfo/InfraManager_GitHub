using System;
using Inframanager;

namespace InfraManager.DAL.ServiceCatalogue
{
    [ObjectClassMapping(ObjectClass.Rule)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Rule_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.Rule_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Rule_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.Rule_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Rule_Properties)]
    public class Rule
    {
        public Rule()
        {
            
        }
        
        public Rule(Rule cloningRule)
        {
            Name = cloningRule.Name;
            Note = cloningRule.Note;
            Sequence = cloningRule.Sequence;
            ServiceTemplateID = cloningRule.ServiceTemplateID;
            Value = cloningRule.Value;
        }
        
        public Guid ID { get; init; }

        public string Name { get; set; }
        
        public string Note { get; set; }

        public int Sequence { get; set; }

        public Guid? ServiceLevelAgreementID { get; set; }
        
        public int? OperationalLevelAgreementID { get; set; }
        
        public Guid? ServiceTemplateID { get; set; }
        
        public byte[] Value { get; set; }

        public byte[] RowVersion { get; init; }

        public virtual ServiceTemplate ServiceTemplate { get; init; }
    }
}
