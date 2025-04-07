using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL
{
    /// <summary>
    /// Этот интерфейс обобщает Finder-ы объектов по их интерфейсу
    /// </summary>
    /// <typeparam name="TAbstract">Базовый тип всех объектов, которые можно найти</typeparam>
    /// <typeparam name="TKey">Тип ключа искомых объектов</typeparam>
    public interface IAbstractFinder<TAbstract, TKey>
    {
        Task<TAbstract> FindAsync(TKey id, CancellationToken cancellationToken = default);
        TAbstract Find(TKey id);
    }
}
