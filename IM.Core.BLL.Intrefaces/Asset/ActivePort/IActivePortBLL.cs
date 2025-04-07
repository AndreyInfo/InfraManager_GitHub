﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.ActivePort;

/// <summary>
/// Бизнес-логика для работы с портами для объекта Сетевое оборудование.
/// </summary>
public interface IActivePortBLL
{
    /// <summary>
    /// Получение таблицы с фильтрацией, пагинацией, поиском и сортировкой.
    /// </summary>
    /// <param name="filter">Фильтр типа <see cref="ActivePortFilter"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Массив данных типа <see cref="ActivePortDetails"/>.</returns>
    Task<ActivePortDetails[]> GetListAsync(ActivePortFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получение порта.
    /// </summary>
    /// <param name="id">Идентификатор порта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные порта <see cref="ActivePortDetails"/>.</returns>
    Task<ActivePortDetails> DetailsAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Создание нового порта.
    /// </summary>
    /// <param name="data">Данные порта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные нового порта типа <see cref="ActivePortDetails"/>.</returns>
    Task<ActivePortDetails> AddAsync(ActivePortData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление порта.
    /// </summary>
    /// <param name="id">Идентификатор порта.</param>
    /// <param name="data">Данные порта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные обновленного порта типа <see cref="ActivePortDetails"/>.</returns>
    Task<ActivePortDetails> UpdateAsync(int id, ActivePortData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление порта.
    /// </summary>
    /// <param name="id">Идентификатор порта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Получение количества портов для объекта Сетевое оборудование.
    /// </summary>
    /// <param name="id">Идентификатор Сетевого оборудования.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Количество портов <see cref="int"/>.</returns>
    Task<int> GetCountPortsAsync(int id, CancellationToken cancellationToken);
}

