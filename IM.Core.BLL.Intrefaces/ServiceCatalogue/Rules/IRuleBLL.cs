using InfraManager.BLL.CrudWeb;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue.Rules
{
    public interface IRuleBLL
    {
        /// <summary>
        /// Добавляет Правило для SLA
        /// </summary>
        /// <param name="rule">Данные правил</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Данные правил</returns>
        Task<RuleDetails> InsertAsync(RuleData rule, CancellationToken cancellationToken = default);


        /// <summary>
        /// Обновляет Правило для SLA
        /// </summary>
        /// <param name="ruleID">Идентификатор правила</param>
        /// <param name="rule">Данные правил</param>
        /// <param name="cancellationToken">токен отмены</param>
        Task UpdateAsync(Guid ruleID, RuleData rule, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает список правил SLA
        /// </summary>
        /// <param name="filter">Фильтр для получения данных</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Список правил</returns>
        Task<RuleDetails[]> ListAsync(RuleFilter filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет правило SLA
        /// </summary>
        /// <param name="id">Идентификатор правила</param>
        /// <param name="cancellationToken">токен отмены</param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получает значения правила SLA
        /// </summary>
        /// <param name="ruleID">Идентификатор правила</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Значения правла</returns>
        Task<RuleValueDetails> GetRuleValueAsync(Guid ruleID, CancellationToken cancellationToken = default);

        /// <summary>
        /// Изменяет значения правила SLA
        /// </summary>
        /// <param name="ruleID">Идентификатор правила</param>
        /// <param name="ruleValue">Данные правил SLA</param>
        /// <param name="cancellationToken">токен отмены</param>
        Task UpdateValueAsync(Guid ruleID, RuleValueDetails ruleValue,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавляет значения правила SLA
        /// </summary>
        /// <param name="ruleID">Идентификатор правила</param>
        /// <param name="ruleValue">Данные правил SLA</param>
        /// <param name="cancellationToken">токен отмены</param>
        Task InsertValueAsync(Guid ruleID, RuleValueDetails ruleValue,
            CancellationToken cancellationToken = default);
    }
}
