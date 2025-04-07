using Inframanager;

namespace InfraManager.DAL.ServiceDesk
{
    /// <summary>
    /// Этот класс представляет сущность Срочность
    /// </summary>
    [OperationIdMapping(ObjectAction.Insert, OperationID.Urgency_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Urgency_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Urgency_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.Urgency_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Urgency_Properties)]
    public class Urgency : SequencedLookup
    {
        protected Urgency() : base()
        {
        }

        public Urgency(string name, int sequence) : base(name, sequence)
        {
        }
    }
}
