namespace InfraManager.CrossPlatform.WebApi.Contracts.Common.Models
{
    public class ObjectClassModel
    {
        public ObjectClassModel(ObjectClassModel orig = null) 
        {
            if (orig != null)
            {
                ObjClassID = orig.ObjClassID;
                ClassID = orig.ClassID;
            }
        }
        /// <summary>
        /// Идентификатор класса объекта.
        /// </summary>
        public int ObjClassID { get; set; }

        /// <summary>
        /// Используется для точного указания класса объекта, когда ObjClassID может соответствовать нескольким классам
        /// </summary>
        public int ClassID { get; set; }
    }
}
