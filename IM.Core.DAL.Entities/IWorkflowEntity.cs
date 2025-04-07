using System;

namespace InfraManager.DAL
{
    public interface IWorkflowEntity : IHaveUtcModifiedDate, IGloballyIdentifiedEntity
    {
        public string EntityStateID { get; set; }
        public string EntityStateName { get; set; }
        public Guid? WorkflowSchemeID { get; set; }
        public string WorkflowSchemeVersion { get; set; }
        string WorkflowSchemeIdentifier { get; set; }
        public string TargetEntityStateID { get; set; } 
        // TODO: При редизайне WorkflowService этот костыль нужно убрать
        // Новое состояние должно сразу писаться в EntityStateID
    }
}
