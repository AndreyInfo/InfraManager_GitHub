using System;

namespace InfraManager.DAL
{
    /// <summary>
    /// Этот класс представляет абстрактную сущность Справочник
    /// </summary>
    public abstract class Lookup
    {
        protected Lookup()
        {
        }

        public Lookup(string name)
        {
            ID = Guid.NewGuid();  
            Name = name;
        }


        /// <summary>
        /// Возвращает идентификатор
        /// </summary>
        public Guid ID { get; init; }

        /// <summary>
        /// Возвращает или задает наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Возвращает или задает идентификатор версии
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
