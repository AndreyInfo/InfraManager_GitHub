using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InfraManager.DAL
{
    /// <summary>
    /// Этот интерфейс описывает посетителя Entry
    /// </summary>
    internal interface IVisitEntityEntry
    {
        /// <summary>
        /// Посещает Entry (до сохранения)
        /// </summary>
        /// <param name="entry">Ссылка на Entry</param>
        /// <param name="cancellationToken"></param>
        Task VisitAsync(EntityEntry entry, CancellationToken cancellationToken);

        /// <summary>
        /// Повторно посетить все Entry после сохранения (т.е. после того как заполнены свойства генерируемые на стороне БД)
        /// </summary>
        /// <param name="cancellationToken"></param>
        Task AfterSaveAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Посещает Entry (до сохранения)
        /// </summary>
        /// <param name="entry">Ссылка на Entry</param>
        void Visit(EntityEntry entry);

        /// <summary>
        /// Повторно посетить все Entry после сохранения (т.е. после того как заполнены свойства генерируемые на стороне БД)
        /// </summary>
        void AfterSave();
    }
}
