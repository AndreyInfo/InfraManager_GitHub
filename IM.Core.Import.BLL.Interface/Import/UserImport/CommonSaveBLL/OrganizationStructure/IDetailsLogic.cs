using InfraManager;

namespace IM.Core.Import.BLL.Interface.Import.UserImport.CommonSaveBLL.OrganizationStructure;

public interface IDetailsLogic<TDetails>
{
    ObjectType GetExcludedFields(TDetails details);
}