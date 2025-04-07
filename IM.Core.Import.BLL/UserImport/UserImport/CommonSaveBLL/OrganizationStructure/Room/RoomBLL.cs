using IM.Core.Import.BLL.Interface;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Location;

namespace IM.Core.Import.BLL
{
    internal class RoomBLL : IRoomBLL, ISelfRegisteredService<IRoomBLL>
    {
        private readonly IReadonlyRepository<Room> _service;
        public RoomBLL(IReadonlyRepository<Room> service)
        {
            _service = service;
        }
        public async Task<Room> GetAsync(Guid? id, CancellationToken cancellationToken)
        {
            return await _service.FirstOrDefaultAsync(x => x.IMObjID == id, cancellationToken);
        }
    }
}
