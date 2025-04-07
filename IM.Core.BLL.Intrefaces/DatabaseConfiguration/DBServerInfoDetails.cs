namespace InfraManager.BLL.DatabaseConfiguration
{
    /// <summary>
    /// Информация о БД
    /// </summary>
    public class DBServerInfoData
    {
        public string ServerName { get; set; }
        public string DataBase { get; set; }
        public string Login { get; init; }
        public string Password { get; init; }
        public string AdditionalField { get; init; }
        public int Port { get; set; }
    }
}
