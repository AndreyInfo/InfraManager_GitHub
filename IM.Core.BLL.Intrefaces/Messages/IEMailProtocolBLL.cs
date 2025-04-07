using InfraManager.WebApi.Contracts.Models.EMailProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages
{
    public interface IEMailProtocolBLL
    {
        /// <summary>
        /// получение списка фактических почтовых адресов для рассылки по запросу 
        /// </summary>
        /// <param name="request">Параметры получения рассылки</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<NotificationReceiverDetails[]> GetEMAilsAsync(EMailListRequest request, CancellationToken cancellationToken);
    }
}
