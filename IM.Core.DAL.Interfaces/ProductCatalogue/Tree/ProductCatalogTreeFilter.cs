using InfraManager.DAL.AccessManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.ProductCatalogue.Tree;
public class ProductCatalogTreeFilter
{
    public ObjectClass ClassID { get; init; }

    public Guid? ParentID { get; init; }

    public bool? UseRemoveCategoryClass { get; init; }

    public Guid? AvailableCategoryID { get; init; }

    public Guid? СustomControlObjectID { get; init; }

    public bool? HasLifeCycle { get; init; }

    public IEnumerable<ObjectClass> AvailableClassID { get; init; }

    public ObjectClass[] RemovedCategoryClassArray { get; init; }

    public IEnumerable<OperationID> AvailableOperationsID { get; init; }

    public ProductTemplate? AvailableTemplateID { get; init; }

    public ObjectClass? AvailableTemplateClassID { get; init; }

    public ObjectClass[] AvailableTemplateClassArray { get; init; }

    public bool ByAccess { get; init; }

    public bool AllClassIDisAvailable => AvailableClassID is null || !AvailableClassID.Any();

    public bool AllRemoveCategoryClassAvailable => RemovedCategoryClassArray is null || !RemovedCategoryClassArray.Any();

}
