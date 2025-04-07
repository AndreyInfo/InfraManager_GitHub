using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL;

internal class TimeZoneObjects<TEntity> : ITimeZoneObjects
where TEntity : class, ITimeZoneObject
{
    // TODO передлать на общее сохранение, когда сделают перестанет отрабатывать WorkflowEntityEventCreator при обновление Заявки  
    private readonly CrossPlatformDbContext _db;
    public TimeZoneObjects(CrossPlatformDbContext db)
    {
        _db = db;
    }

    public async Task UpdateTimeZoneObjectsAsync(string timeZoneID, CancellationToken cancellationToken)
    {
        //TODO
        // В EF нельзя обновлять, не получив сущность в контекст
        // В EF7 можно делать update не вытягивая сущность, EF генерит скрипт Update [Table] SET ..... WHERE .....
        var objects = await _db.Set<TEntity>().ToArrayAsync(cancellationToken);
        objects.ForEach(c => c.ChangeTimeZone(timeZoneID));
        //TODO перевести на общее сохранение когда
        // переделают общее сохранение, перестанет падать при отрабатывать WorkflowEntityEventCreator при обновление Заявки
        await _db.SaveChangesAsync(cancellationToken);
    }
}
