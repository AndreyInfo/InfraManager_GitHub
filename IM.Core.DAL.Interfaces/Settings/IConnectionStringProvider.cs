namespace InfraManager.DAL.Settings
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString();

        (string Server, string Database, string Login, string Password, int Port, int CommandTimeout) GetConnectionObject();

        void ChangeConnectionString(string connectionString);

        string BuildConnectionString(string server, string database, string login, string password,
            string additionalField, int port);
    }
}
