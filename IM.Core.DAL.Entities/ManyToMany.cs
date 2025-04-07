using System;

namespace InfraManager.DAL
{
    /// <summary>
    /// Этот класс представляет связку многие ко многим между TParent и TReference
    /// </summary>
    /// <typeparam name="TParent">Тип родительской сущности (в которой будет навигационная коллекция этого типа)</typeparam>
    /// <typeparam name="TReference">Тип дочерней сущности</typeparam>
    public class ManyToMany<TParent, TReference> where TParent : class where TReference : class
    {
        protected ManyToMany()
        {
        }

        public ManyToMany(TReference reference) : this()
        {
            Reference = reference;
        }
        
        /// <summary>
        /// Возвращает идентификатор связи TParent - TReference
        /// </summary>
        public long ID { get; }

        //TODO: Выпилить
        /// <summary>
        /// Возвращает ссылку на родительский элемент
        /// </summary>
        [Obsolete]
        public virtual TParent Parent { get; }
        
        /// <summary>
        /// Возвращает ссылку на дочерний элемент
        /// </summary>
        public virtual TReference Reference { get; }
    }
}
