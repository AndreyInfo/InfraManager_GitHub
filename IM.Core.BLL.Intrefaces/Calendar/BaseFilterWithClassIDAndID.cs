using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Calendar
{
    public class BaseFilterWithClassIDAndID<TID> : BaseFilter where TID : struct
    {
        public ObjectClass? ClassID { get; set; }

        public TID? ObjectID { get; set; }
    }
}
