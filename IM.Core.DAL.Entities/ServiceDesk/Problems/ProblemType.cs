using Inframanager;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.ServiceCatalogue;
using System;

namespace InfraManager.DAL.ServiceDesk;

/// <summary>
/// Этот класс представляет сущность "Тип проблемы"
/// </summary>
[ObjectClassMapping(ObjectClass.ProblemType)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ProblemType_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.ProblemType_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ProblemType_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ProblemType_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.ProblemType_Properties)]
public class ProblemType : Lookup, IMarkableForDelete, IFormBuilder
{
    protected ProblemType() : base()
    {
    }

    public ProblemType(string name) : base(name)
    {
    }
    
    public ProblemType(string name, ProblemType parentProblemType) : base(name)
    {
        Parent = parentProblemType;
    }

    public Guid? ParentProblemTypeID { get; set; }

    public byte[] Image { get; set; }

    public bool Removed { get; private set; }

    public void MarkForDelete() => Removed = true;

    public string WorkflowSchemeIdentifier { get; set; }

    public Guid? FormID { get; set; }

    public virtual Form Form { get; }

    public string ImageName { get; set; }

    public virtual ProblemType Parent { get; private set; }

    public virtual WorkFlowScheme WorkflowScheme { get; set; }

    public string GetWorkflowSchemeIdentifier()
    {
        return string.IsNullOrWhiteSpace(WorkflowSchemeIdentifier) 
            ? Parent?.GetWorkflowSchemeIdentifier()
            : WorkflowSchemeIdentifier;
    }

    public string FullName => Parent == null ? Name : $"{Parent.FullName}\\{Name}";

    public static string GetFullProblemTypeName(Guid id) =>
        throw new NotSupportedException();
}
