using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Configuration;

namespace InfraManager.DAL
{
    public interface IDbConfiguration
    {
        /// <summary>
        /// Проверяет подключение к определенной базе данных
        /// </summary>
        /// <param name="server">Сервер БД</param>
        /// <param name="databaseName">Название базы данных</param>
        /// <param name="login">Логин пользователя для входа. При login = null будет использоваться страндартный логин IM</param>
        /// <param name="password">Пароль пользователя для входа. При password = null будет использоваться страндартный пароль IM</param>
        /// <param name="additionalField">Доп. поля</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        Task<bool> CheckConnectionAsync(string server, int port, string databaseName = null, string login = null,
            string password = null, string additionalField = null, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Проверяет подключение к определенной базе данных
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        Task<bool> CheckConnectionAsync(string connectionString, CancellationToken cancellationToken = default);

        /// <summary>
        /// Создает строку подключения для базы данных
        /// </summary>
        /// <param name="server">Сервер БД</param>
        /// <param name="databaseName">Название базы данных</param>
        /// <param name="login">Логин пользователя для входа. При login = null будет использоваться страндартный логин IM</param>
        /// <param name="password">Пароль пользователя для входа. При password = null будет использоваться страндартный пароль IM</param>
        /// <param name="additionalField">Дополнительные поля для строки подключения(ssl mode etc.)</param>
        string BuildConnectionString(string server, int port, string databaseName = null, string login = null,
            string password = null, string additionalField = null);

        /// <summary>
        /// При удачном подключении загружает возвращает все базы IM
        /// </summary>
        /// <param name="server">Сервер БД</param>
        /// <param name="databaseName">Название базы данных</param>
        /// <param name="login">Логин пользователя для входа. При login = null будет использоваться страндартный логин IM</param>
        /// <param name="password">Пароль пользователя для входа. При password = null будет использоваться страндартный пароль IM</param>
        /// <param name="additionalField">Доп. поля</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        Task<string[]> LoadDataBasesAsync(string server, int port, string databaseName = null, string login = null,
            string password = null, string additionalField = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Восстанавливает базу данных
        /// </summary>
        /// <param name="server">Сервер БД</param>
        /// <param name="dbRestoreType">Тип базы данных для восстановления</param>
        /// <param name="databaseName">Название базы данных</param>
        /// <param name="login">Логин пользователя для входа. При login = null будет использоваться страндартный логин IM</param>
        /// <param name="password">Пароль пользователя для входа. При password = null будет использоваться страндартный пароль IM</param>
        /// <param name="cancellationToken"></param>
        Task RestoreDatabaseAsync(string server, int port, DbRestoreType dbRestoreType, string databaseName = null,
            string login = null, string password = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Подключается к выбранной бд(изменяет appsettings.(stage.)json и OldDataSourceLocator
        /// </summary>
        /// <param name="server">Сервер БД</param>
        /// <param name="databaseName">Название базы данных</param>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="additionalField">Дополнительные поля для строки подключения(ssl mode etc.)</param>
        void ConnectToDatabase(string server, int port, string databaseName = null, string login = null,
            string password = null,
            string additionalField = null);
    }
}