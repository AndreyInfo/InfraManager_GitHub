using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.AppSettings;
using InfraManager.DAL.Configuration;

namespace InfraManager.BLL.Settings
{
    public interface IAppSettingsBLL
    {
        Task<string[]> GetDatabaseListAsync(string serverName, string login, string password, string additionalField,
            int port, CancellationToken cancellationToken = default);

        Task RestoreDatabaseAsync(string serverName,int port, string dataBase, string login, string password,
            DbRestoreType dbType, CancellationToken cancellationToken = default);

        void ConnectToDatabase(string serverName,int port, string dataBase, string login = null, string password = null,
            string additionalField = null);
        
        /// <summary>
        /// Возвращает данные к подключенной субд
        /// </summary>
        ConnectedDatabaseConfiguration GetDatabaseConfiguration();

        /// <summary>
        /// Возвращает настройки системы
        /// </summary>
        /// <param name="validate">Нужно ли валидировать права?(для получения настроек на бэкэнде)</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task<SystemSettingData> GetConfigurationAsync(bool validate = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновляет конфигурацию settings в appsettings 
        /// </summary>
        /// <param name="data">Новые настройки системы</param>
        /// <param name="cancellationToken">Токен отмены</param>
        Task UpdateConfigurationAsync(SystemSettingData data,
            CancellationToken cancellationToken = default);
    }
}
