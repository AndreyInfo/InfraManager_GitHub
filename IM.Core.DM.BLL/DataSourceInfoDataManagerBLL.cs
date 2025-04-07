using IM.Core.DM.BLL.Interfaces;
using InfraManager;
using InfraManager.Core.Extensions;
using InfraManager.DAL;
using System;
using System.Linq;
using System.Transactions;

namespace IM.Core.DM.BLL
{
    internal class DataSourceInfoDataManagerBLL : IDataSourceInfoDataManagerBLL, ISelfRegisteredService<IDataSourceInfoDataManagerBLL>
    {
        private readonly IUnitOfWork saveChangesCommand;
        private readonly IReadonlyRepository<DBInfo> dbInfoRepository;
        private readonly IRepository<DataSourceInfo> dataSourceRepository;

        public DataSourceInfoDataManagerBLL(
                    IUnitOfWork saveChangesCommand,
                    IRepository<DataSourceInfo> dataSourceRepository,
                    IReadonlyRepository<DBInfo> dbInfoRepository)
        {
            this.saveChangesCommand = saveChangesCommand;
            this.dataSourceRepository = dataSourceRepository;
            this.dbInfoRepository = dbInfoRepository;
        }

        public Guid RegisterOwner(Guid ownerID, string processName, string machineName, string ipAddresses)
        {
            Guid id;
            var dataSourceInfo = dataSourceRepository.Query()
                                          .FirstOrDefault(x => x.OwnerID == ownerID);
            if (dataSourceInfo == null)
            {
                var dbInfo = dbInfoRepository.Query().First();
                id = dbInfo.ID;
                dataSourceInfo = new DataSourceInfo()
                {
                    DataSourceID = id,
                    OwnerID = ownerID,
                    ProcessName = processName.Truncate(50),
                    MachineName = machineName.Truncate(50),
                    IPAddresses = ipAddresses.Truncate(500),
                    UtcCheckedAt = DateTime.UtcNow
                };
                dataSourceRepository.Attach(dataSourceInfo);
            }
            else
            {
                id = dataSourceInfo.DataSourceID;
                dataSourceInfo.UtcCheckedAt = DateTime.UtcNow;
            }
            saveChangesCommand.Save(IsolationLevel.RepeatableRead);
            return id;
        }

        public int CheckDataSource(Guid dbID, Guid ownerID)
        {
            var utcNow = DateTime.UtcNow;
            var dataSourceInfo = dataSourceRepository.Query()
                     .FirstOrDefault(x => x.DataSourceID == dbID && x.OwnerID == ownerID);
            if (dataSourceInfo != null)
                dataSourceInfo.UtcCheckedAt = utcNow;

            utcNow = utcNow.AddHours(-1);
            var removingObjects = dataSourceRepository.Query()
                     .Where(x => x.UtcCheckedAt < utcNow)
                     .ToList();
            if (removingObjects.Count != 0)
                removingObjects.ForEach(x => dataSourceRepository.Delete(x));

            saveChangesCommand.Save();
            return (dataSourceInfo != null) ? 1 : 0;
        }

        public Tuple<string, string, string> GetOwnerInfo(Guid ownerID)
        {
            var dataSourceInfo = dataSourceRepository.Query()
                .FirstOrDefault(x => x.OwnerID == ownerID);

            if (dataSourceInfo is null)
                return null;

            return Tuple.Create(dataSourceInfo.ProcessName, dataSourceInfo.MachineName, dataSourceInfo.IPAddresses);
        }
    }
}
