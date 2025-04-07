using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Models
{
    public class ObjectIDModel : ObjectClassModel
    {
        public ObjectIDModel(ObjectIDModel orig = null) :base(orig)
        {
            if (orig != null)
                ID = orig.ID;
        }
        /// <summary>
        /// Идентификатор объекта, для котрого производится изменение
        /// </summary>
        public Guid ID { get; set; }
    }
}
