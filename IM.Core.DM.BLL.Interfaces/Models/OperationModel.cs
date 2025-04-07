namespace IM.Core.DM.BLL.Interfaces.Models
{
    public class OperationModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public int ClassId { get; set; }

        public string ClassName { get; set; }
    }
}
