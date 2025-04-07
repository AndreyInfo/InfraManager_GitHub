using System;
using System.Collections.Generic;

namespace InfraManager.Core.Xml
{
    /// <summary>
    /// Класс, реализующий модель коллекции атрибутов <see cref="T:InfraManager.Core.Xml.XmlAttribute"/>, доступной только для чтения
    /// </summary>
    [Serializable]
    public sealed class XmlAttributeCollection : IEnumerable<XmlAttribute>
    {
        #region Fields
        /// <summary>
        /// Коллекция моделей атрибутов xml, которую необходимо предоставить для чтения
        /// </summary>
        private readonly List<XmlAttribute> _collection;
        #endregion

        #region Properties
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
        public XmlAttribute this[int index] { get { return _collection[index]; } }
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlAttributes">Коллекция атрибутов xml, которую необходимо предоставить для чтения</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public XmlAttributeCollection(IEnumerable<XmlAttribute> xmlAttributes)
        {
            if (xmlAttributes == null)
                throw new ArgumentNullException("xmlElements");
            _collection = new List<XmlAttribute>(xmlAttributes);
        }
        #endregion

        #region IEnumerable<XmlElement> Members
        public IEnumerator<XmlAttribute> GetEnumerator()
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
