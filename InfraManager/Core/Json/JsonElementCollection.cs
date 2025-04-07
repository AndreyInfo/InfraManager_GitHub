using System;
using System.Collections.Generic;

namespace InfraManager.Core.Json
{
    /// <summary>
    /// Класс, реализующий модель коллекции элементов <see cref="T:InfraManager.Core.Json.JsonElement"/>, доступной только для чтения
    /// </summary>
    [Serializable]
    public sealed class JsonElementCollection : IEnumerable<JsonElement>
    {
        #region Fields
        /// <summary>
        /// Коллекция моделей элементов json, которую необходимо предоставить для чтения
        /// </summary>
        private readonly List<JsonElement> _collection;
        #endregion

        #region Properties
        #region Length
        /// <summary>
        /// Число элементов в коллекции
        /// </summary>
        public int Length { get { return this.Count; } }
        #endregion

        #region Count
        /// <summary>
        /// Число элементов в коллекции
        /// </summary>
        public int Count { get { return _collection.Count; } }
        #endregion
        #endregion

        #region Indexators
        /// <summary>
        /// Возвращает элемент коллекции, находящийся в коллекции в позиции под заданным индексом
        /// </summary>
        /// <param name="index">Индекс элемента коллекции</param>
        /// <returns>Элемент коллекции, находящийся в коллекции в позиции под заданным индексом</returns>
        public JsonElement this[int index] { get { return _collection[index]; } }
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonElements">Коллекция моделей элементов json, которую необходимо предоставить для чтения</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public JsonElementCollection(IEnumerable<JsonElement> jsonElements)
        {
            if (jsonElements == null)
                throw new ArgumentNullException("jsonElements");
            _collection = new List<JsonElement>(jsonElements);
        }
        #endregion

        #region IEnumerable<JsonElement> Members
        public IEnumerator<JsonElement> GetEnumerator()
        {
            foreach (var element in _collection)
                yield return element;
        }
        #endregion

        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
        #endregion
    }
}
