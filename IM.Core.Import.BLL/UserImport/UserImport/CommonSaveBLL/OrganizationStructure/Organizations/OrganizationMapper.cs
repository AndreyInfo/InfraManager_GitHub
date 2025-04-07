using AutoMapper;
using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.UserImport.CommonSaveBLL.OrganizationStructure;
using InfraManager;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Import;

internal class OrganizationMapper : ImportMapper<OrganizationDetails, Organization>, ISelfRegisteredService<IImportMapper<OrganizationDetails, Organization>>
{
    public OrganizationMapper(IDetailsLogic<OrganizationDetails> detailsLogic) : base(detailsLogic)
    {
    }
    
    protected override Func<OrganizationDetails, OrganizationDetails?>? Recursion => null;

    protected override Action<Organization, Organization?>? SetRecursion => null;

    protected override void SetIgnoreFields(ObjectType flags, IMappingExpression<OrganizationDetails, Organization> map)
    {
        IgnoreFieldsIf(flags, ObjectType.OrganizationName, map, x=>x.Name);
        IgnoreFieldsIf(flags, ObjectType.OrganizationNote, map, x => x.Note);
        IgnoreFieldsIf(flags, ObjectType.OrganizationExternalID, map, x => x.ExternalId);

    }


   
}