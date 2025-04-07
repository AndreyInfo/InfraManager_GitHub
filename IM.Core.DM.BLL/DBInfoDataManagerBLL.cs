using IM.Core.DM.BLL.Interfaces;
using IM.Core.DM.BLL.Interfaces.Models;
using InfraManager;
using InfraManager.DAL;
using System.Linq;

namespace IM.Core.DM.BLL
{
    internal class DBInfoDataManagerBLL : IDBInfoDataManagerBLL, ISelfRegisteredService<IDBInfoDataManagerBLL>
    {
        private readonly IReadonlyRepository<DBInfo> dbInfoRepository;

        public DBInfoDataManagerBLL(IReadonlyRepository<DBInfo> dbInfoRepository)
        {
            this.dbInfoRepository = dbInfoRepository;
        }

        public DBInfoModel Get()
        {
            return dbInfoRepository.Query()
                    .Select(x => new DBInfoModel() { 
                        ID = x.ID,
                        IsCentral = x.IsCentral,
                        IsPeripheral = x.IsPeripheral,
                        SIDControl = x.SIDControl,
                        Version = x.Version
                    })
                    .First();
        }
    }
}
