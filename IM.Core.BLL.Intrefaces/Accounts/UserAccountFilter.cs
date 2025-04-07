using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Accounts
{
    /// <summary>
    /// Фильтр для поиска учетных записей
    /// </summary>
    public class UserAccountFilter : BaseFilter
    {
        /// <summary>
        /// Указывает, кодированные или раскодированные учетные данные должны быть 
        /// </summary>
        public bool IsDecoded { get; init; }
    }
}
