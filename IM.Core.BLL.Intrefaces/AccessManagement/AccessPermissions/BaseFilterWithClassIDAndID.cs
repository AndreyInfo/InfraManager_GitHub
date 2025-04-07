using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.AccessManagement.AccessPermissions
{
    public class BaseFilterWithClassIDAndID<TID> : BaseFilter
    {
        public ObjectClass ClassID { get; init; }

        public TID ObjectID { get; init; }
    }
}
