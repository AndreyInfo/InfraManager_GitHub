
	namespace InfraManager.BLL.ActiveDirectory.Import;
	
		public interface IUIADClassBLL
		{
		/// <summary>
        /// Получение nаблицы наборов классов импорта ldap.
        /// </summary>
        /// <param name="filter">Фильтр таблицы</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Таблица наборов классов импорта ldap</returns>
        Task<UIADClassOutputDetails[]> GetDetailsArrayAsync(UIADClassFilter filter,
            CancellationToken cancellationToken);

        /// <summary>
        /// Получение данных для набора классов импорта ldap каталога продуктов 
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Детализация набора классов импорта ldap</returns>
        Task<UIADClassOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Вставляет набор классов импорта ldap в указанную категорию 
        /// </summary>
        /// <param name="data">Данные для встаки</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<UIADClassOutputDetails> AddAsync(UIADClassDetails data,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновление набора классов импорта ldap ссответствующего идентификатору
        /// </summary>
        /// <param name="id">Идентификатор набора классов импорта ldap</param>
        /// <param name="data">Новые данные для набора классов импорта ldap с тем же идентификатором</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task<UIADClassOutputDetails> UpdateAsync(Guid id,
            UIADClassDetails data,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаление набора классов импорта ldap
        /// </summary>
        /// <param name="id">Идентификатор набора классов импорта ldap</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

	}
	