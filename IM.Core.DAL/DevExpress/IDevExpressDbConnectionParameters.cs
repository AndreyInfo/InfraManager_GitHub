using DevExpress.DataAccess.ConnectionParameters;

namespace InfraManager.DAL.DevExpress;

/// <summary>
/// Создает параметры для подключения к бд для DevExpress
/// </summary>
public interface IDevExpressDbConnectionParameters
{
    SqlServerConnectionParametersBase GetConnectionParameters(
        (string Server, string Database, string Login, string Password, int Port, int CommandTimeout)
            connectionParameters);
}