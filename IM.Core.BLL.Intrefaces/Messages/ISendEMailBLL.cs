using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages
{
    /// <summary>
    /// Обспечиват отправку почты через Почтовый сервис
    /// </summary>
    public interface ISendEMailBLL
    {
        /// <summary>
        /// Отправляет почту, включай файлы как вложения, информация о которых передается
        /// </summary>
        /// <param name="data">Письмо для отправки</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SendEMailAsync(SendEMailData data, CancellationToken cancellationToken);
    }
}
