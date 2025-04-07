using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Inframanager;
using InfraManager.DAL.FormBuilder;

namespace InfraManager.DAL.ProductCatalogue.LifeCycles;

[ObjectClassMapping(ObjectClass.LifeCycle)]
[OperationIdMapping(ObjectAction.Insert, OperationID.LifeCycle_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.LifeCycle_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.LifeCycle_Delete)]
[OperationIdMapping(ObjectAction.InsertAs, OperationID.LifeCycle_AddAs)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.LifeCycle_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.LifeCycle_Properties)]
public class LifeCycle : IMarkableForDelete
{
    protected LifeCycle()
    {

    }

    public LifeCycle(string name, LifeCycleType type)
    {
        ID = Guid.NewGuid();
        Name = name;
        Type = type;
    }

    public Guid ID { get; init; }
    public string Name { get; init; }
    public bool IsFixed { get; init; }
    public LifeCycleType Type { get; init; }
    public bool Removed { get; private set; }
    public Guid? FormID { get; set; }
    public byte[] RowVersion { get; init; }

    public virtual Form Form { get; init; }
    public virtual ICollection<LifeCycleState> LifeCycleStates { get; init; }
    public virtual IEnumerable<ProductCatalogType> ProductCatalogType { get; init; }

    public static Expression<Func<LifeCycle, bool>> IsNotRemoved => x => !x.Removed;

    public void MarkForDelete()
    {
        Removed = true;
    }
}
