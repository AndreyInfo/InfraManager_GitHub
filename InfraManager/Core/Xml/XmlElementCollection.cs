using System;
using System.Collections.Generic;

namespace InfraManager.Core.Xml
{
    /// <summary>
    /// Класс, реализующий модель коллекции элементов <see cref="T:InfraManager.Core.Xml.XmlElement"/>, доступной только для чтения
    /// </summary>
    [Serializable]
    public sealed class XmlElementCollection : IEnumerable<XmlElement>
    {
        #region Fields
        /// <summary>
        /// Коллекция моделей элементов xml, которую необходимо предоставить для чтения
        /// </summary>
        private readonly List<XmlElement> _collection;
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
        public XmlElement this[int index] { get { return _collection[index]; } }
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlElements">Коллекция моделей элементов xml, которую необходимо предоставить для чтения</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public XmlElementCollection(IEnumerable<XmlElement> xmlElements)
        {
            if (xmlElements == null)
                throw new ArgumentNullException("xmlElements");
            _collection = new List<XmlElement>(xmlElements);
        }
        #endregion

        #region IEnumerable<XmlElement> Members
        public IEnumerator<XmlElement> GetEnumerator()
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
