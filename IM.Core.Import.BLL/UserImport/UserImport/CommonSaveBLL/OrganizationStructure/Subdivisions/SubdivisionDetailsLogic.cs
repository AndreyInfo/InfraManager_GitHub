using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.UserImport.CommonSaveBLL.OrganizationStructure;
using InfraManager;

namespace IM.Core.Import.BLL.UserImport.UserImport.CommonSaveBLL.OrganizationStructure.Subdivisions;

public class SubdivisionDetailsLogic:IDetailsLogic<ISubdivisionDetails>, ISelfRegisteredService<IDetailsLogic<ISubdivisionDetails>>
{
    public ObjectType GetExcludedFields(ISubdivisionDetails details)
    {
        ObjectType flags = ObjectType.Nothing;
        if (!SubdivisionSimpleNameKey.IsSet(details.Name))
            flags |= ObjectType.SubdivisionName;
        return flags;
    }
}