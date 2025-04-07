namespace InfraManager.BLL.Authentication
{
    /// <summary>
    /// Сервис аутентификации
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Метод проверки существования пользователя в LDAP
        /// </summary>
        /// <param name="login">Логин в LDAP</param>
        /// <param name="password">Пароль в LDAP</param>
        /// <returns>Результат аутентификации(true, если пользователь найден под такими данными в LDAP, иначе false)</returns>
        public bool IsValid(string login, string password);
    }
}
