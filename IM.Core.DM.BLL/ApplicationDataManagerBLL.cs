using IM.Core.DM.BLL.Interfaces;
using InfraManager;
using InfraManager.DAL;
using System;
using System.Linq;

namespace IM.Core.DM.BLL
{
    internal class ApplicationDataManagerBLL : IApplicationDataManagerBLL, ISelfRegisteredService<IApplicationDataManagerBLL>
	{
		private readonly IReadonlyRepository<DBInfo> dbInfoRepository;

		public ApplicationDataManagerBLL(IReadonlyRepository<DBInfo> dbInfoRepository)
        {
			this.dbInfoRepository = dbInfoRepository;
        }

        public string GetDBVersion()
        {
			var version = "";
			try
			{
				var dbInfo = dbInfoRepository.Query().FirstOrDefault();
				if (dbInfo != null)
					version = dbInfo.Version;
			}
			catch (Exception ex)
			{
				// empty
			}
			return version;
		}
    }
}
