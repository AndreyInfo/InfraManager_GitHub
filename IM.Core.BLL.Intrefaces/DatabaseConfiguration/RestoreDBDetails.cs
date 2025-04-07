using InfraManager.DAL.Configuration;

namespace InfraManager.BLL.DatabaseConfiguration
{
    public sealed class RestoreDBData : DBServerInfoData
    {
        public DbRestoreType DbRestoreType { get; init; }
    }
}
