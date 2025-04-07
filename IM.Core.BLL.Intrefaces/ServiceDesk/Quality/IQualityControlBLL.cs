using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Quality
{
    public interface IQualityControlBLL
    {
        /// <summary>
        /// Добавляет запись контроля качества
        /// </summary>
        /// <param name="data">Данные записи</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные новой заявки</returns>
        Task<QualityControlDetails> AddAsync(QualityControlData data, CancellationToken cancellationToken = default);

        Task<DateTime?> GetLastByCallAsync(Guid callID, CancellationToken cancellationToken = default);

    }
}
