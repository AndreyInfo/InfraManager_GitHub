
namespace IM.Core.Import.BLL.Interface
{
    public class PositionModel
    {
        public string Name { get; set; }
        public int? ComplementaryId { get; set; }
        public Guid? IMObjID { get; private set; }
    }
}
