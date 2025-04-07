using InfraManager.DAL;

namespace InfraManager.BLL.UserUniqueFiltrations;

/// <summary>
/// Интерфейс позволяет создавать Query с учетом пользовательской фильтрации
/// </summary>
public interface IUserFiltrationQueryBuilder<Entity, Table>
{
    /// <summary>
    ///  Создает <see cref="IExecutableQuery"/> с учетом пользовательских фильтров
    /// </summary>
    /// <param name="columnsParams">Информация с пользовательской фильтрацией</param>
    /// <param name="query">Query, в которую будет добавлена фильтрация(необязательный параметр, нужен в случае если
    /// у вас есть другая Query, в которую нужно добавить фильтрацию)</param>
    public IExecutableQuery<Entity> Build(PersonalUserFiltrationItem[] columnsParams,
        IExecutableQuery<Entity> query = null);
}