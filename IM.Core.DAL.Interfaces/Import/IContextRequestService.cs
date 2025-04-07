using System.Data.Common;

namespace InfraManager.DAL.Import;

public interface IContextRequestService
{
    DbConnection GetDbConnection();
}