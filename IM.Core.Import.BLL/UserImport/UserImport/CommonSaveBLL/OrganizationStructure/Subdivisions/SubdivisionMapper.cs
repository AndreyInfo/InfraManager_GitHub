using AutoMapper;
using IM.Core.Import.BLL.Import.Array;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Import.Models;
using IM.Core.Import.BLL.Interface.Import.UserImport.CommonSaveBLL.OrganizationStructure;
using InfraManager;
using InfraManager.DAL.Import;
using InfraManager.DAL.OrganizationStructure;

namespace IM.Core.Import.BLL.Import;

internal class SubdivisionMapper : ImportMapper<ISubdivisionDetails,Subdivision>,ISelfRegisteredService<IImportMapper<ISubdivisionDetails,Subdivision>>
{
    public SubdivisionMapper(IDetailsLogic<ISubdivisionDetails> detailsLogic) : base(detailsLogic)
    {
    }
    
    protected override void AdditionalInit(IMapper mapper, ISubdivisionDetails detail, Subdivision entity,
        Dictionary<ISubdivisionDetails, Subdivision> mappingDictionary)
    {
        if (detail is ISubdivisionHierarchyDetails hierarchyDetails)
        {
            var parentSubdivision = hierarchyDetails.ParentSubdivision;
            if (parentSubdivision == null)
                return;
            if (mappingDictionary.ContainsKey(parentSubdivision))
                entity.ParentSubdivision = mappingDictionary[parentSubdivision];
            else
            {
                entity.ParentSubdivision = mapper.Map<Subdivision>(parentSubdivision);
                mappingDictionary[parentSubdivision] = entity.ParentSubdivision;
            }
        }
    }

    protected override Func<ISubdivisionDetails, ISubdivisionDetails?>? Recursion => x=>x?.ParentSubdivision;

    protected override Action<Subdivision, Subdivision?>? SetRecursion => (x,y)=>x.ParentSubdivision =y;

    protected override void SetIgnoreFields(ObjectType flags, IMappingExpression<ISubdivisionDetails, Subdivision> map)
    {
        IgnoreFieldsIf(flags,ObjectType.SubdivisionName, map, x=>x.Name);
        IgnoreFieldsIf(flags, ObjectType.SubdivisionNote, map, x => x.Note);
        IgnoreFieldsIf(flags, ObjectType.SubdivisionExternalID, map, x => x.ExternalID);
        IgnoreFieldsIf(flags, ObjectType.SubdivisionOrganization, map, x => x.Organization);
        IgnoreFieldsIf(flags, ObjectType.SubdivisionOrganizationExternalID, map, x => x.OrganizationID);
        IgnoreFieldsIf(flags, ObjectType.SubdivisionParentExternalID, map, x => x.SubdivisionID);
        Ignore(map, x => x.It);
        Ignore(map,x=>x.IsLockedForOsi);
        Ignore(map, x => x.ComplementaryID);
        Ignore(map, x => x.PeripheralDatabaseID);
        Ignore(map, x => x.ChildSubdivisions);
        Ignore(map, x => x.Users);
        Ignore(map, x => x.ParentSubdivision);
    }


   
}