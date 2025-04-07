using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.UserImport.CommonSaveBLL.OrganizationStructure;
using InfraManager;

namespace IM.Core.Import.BLL.UserImport.UserImport.CommonSaveBLL.OrganizationStructure.Organizations;

public class OrganizationDetailsLogic:IDetailsLogic<OrganizationDetails>,ISelfRegisteredService<IDetailsLogic<OrganizationDetails>>
{
    public ObjectType GetExcludedFields(OrganizationDetails details)
    {
        ObjectType flags = ObjectType.Nothing;
        if (!OrganizationNameKey.IsSet(details.Name))
            flags |= ObjectType.OrganizationName;
        return flags;
    }
}