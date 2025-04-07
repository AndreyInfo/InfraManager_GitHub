using InfraManager.BLL.Settings;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages
{
    public interface IMessageByEmailBLL
    {
        Task<MessageByEmailDetails> AddAsync(MessageByEmailData messageByEmail, CancellationToken cancellationToken = default);
        Task<MessageByEmailDetails> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<byte[]> GetRulesSettingsValueAsync(CancellationToken cancellationToken = default);
        Task<OperationResult> SetRulesSettingsValueAsync(List<MessageProcessingRule> messageProcessingRuleList, CancellationToken cancellationToken = default);
      
        /// <summary>
        /// Возвращает флаг использованиия правил удаления цитирования
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> GetCitateTrimmerUsingAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Сохраняет флаг использованиия правил удаления цитирования
        /// </summary>
        /// <param name="citateTrimmerUsing">флаг использованиия правил удаления цитирования</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OperationResult> SetCitateTrimmerUsingAsync(bool citateTrimmerUsing, CancellationToken cancellationToken = default);
        /// <summary>
        /// Обновлет данные сообщения
        /// </summary>
        /// <param name="id">Идентификатор сообщения</param>
        /// <param name="messageByEmailData">Данные сообщения для обновления</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MessageByEmailDetails> UpdateAsync(Guid id, MessageByEmailData model, CancellationToken cancellationToken);
    }
}
