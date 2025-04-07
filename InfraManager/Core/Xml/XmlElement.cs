using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace InfraManager.Core.Xml
{
    /// <summary>
    /// Внутренний вспомогательный класс, реализующий модель контейнера для данных элемента xml, поддерживающий сериализацию
    /// </summary>
    [Serializable]
    public sealed class XmlElement
    {
        #region Fields
        /// <summary>
        /// Расширенное имя элемента 
        /// </summary>
        private readonly string _name;
        /// <summary>
        /// Словарь атрибутов элемента
        /// </summary>
        private readonly List<XmlAttribute> _attributes;
        /// <summary>
        /// Список дочерних элементов
        /// </summary>
        private readonly List<XmlElement> _elements;
        /// <summary>
        /// Предок элемента
        /// </summary>
        private readonly XmlElement _ancestor;
        /// <summary>
        /// Значение
        /// </summary>
        private readonly string _value;
        #endregion

        #region Properties
        #region Name
        /// <summary>
        /// Расширенное имя элемента 
        /// </summary>
        public string Name { get { return _name; } }
        #endregion

        #region Ancestor
        /// <summary>
        /// Предок элемента
        /// </summary>
        public XmlElement Ancestor { get { return _ancestor; } }
        #endregion

        #region Attributes
        /// <summary>
        /// Словарь атрибутов элемента
        /// </summary>
        private List<XmlAttribute> Attributes { get { return _attributes; } }
        #endregion

        #region Elements
        /// <summary>
        /// Список дочерних элементов
        /// </summary>
        private List<XmlElement> Elements { get { return _elements; } }
        #endregion

        #region HasAttributes
        /// <summary>
        /// Флаг, указывающий есть ли у элемента атрибуты
        /// </summary>
        public bool HAsAttributes { get { return this.Attributes.Count != 0; } }
        #endregion

        #region HasElements
        /// <summary>
        /// Флаг, указывающий есть ли дочерние элементы
        /// </summary>
        public bool HasElements { get { return this.Elements.Count != 0; } }
        #endregion

        #region HasValue
        /// <summary>
        /// Флаг, указывающий, есть ли у элемента значение
        /// </summary>
        public bool HasValue { get { return !this.HasElements && this.Value != null; } }
        #endregion

        #region Value
        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get { return _value; } }
        #endregion
        #endregion

        #region Indexators
        /// <summary>
        /// Возвращает элемент коллекции, находящийся в коллекции вложенных элементов в позиции под заданным индексом
        /// </summary>
        /// <param name="index">Индекс элемента коллекции</param>
        /// <returns>Элемент коллекции, находящийся в коллекции вложенных элементов в позиции под заданным индексом</returns>
        public XmlElement this[int index] { get { return this.Elements[index]; } }
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xElement">Элемент, который необходимо преобразовать</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public XmlElement(XElement xElement)
            : this(xElement, null)
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xElement">Элемент, который необходимо преобразовать</param>
        /// <param name="ancestor">Предок элемента</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public XmlElement(XElement xElement, XmlElement ancestor)
        {
            if (xElement == null)
                throw new ArgumentNullException("xElement");
            _ancestor = ancestor;
            _name = xElement.Name.ToString();
            _attributes = new List<XmlAttribute>();
            foreach (var a in xElement.Attributes())
                _attributes.Add(new XmlAttribute(a));
            _elements = new List<XmlElement>();
            foreach (var x in xElement.Elements())
                _elements.Add(new XmlElement(x));
            if (xElement.HasElements || xElement.IsEmpty)
                _value = null;
            else
                _value = xElement.Value;
        }
        #endregion

        #region method CheckHasAttributeByName
        /// <summary>
        /// Возвращает флаг, указывающий содержит ли элемент атрибут с указанным именем
        /// </summary>
        /// <param name="name">Наименование атрибута</param>
        /// <returns>True, если содержит, в противном случае - False</returns>
        public bool CheckHasAttributeByName(string name)
        {
            return this.Attributes.Any(a => a.Name == name);
        }
        #endregion

        #region method CheckHasElementsByName
        /// <summary>
        /// Возвращает флаг, указывающий содержит ли элемент дочерние элементы с указанным именем
        /// </summary>
        /// <param name="name">Наименование элементов</param>
        /// <returns>True, если содержит, в противном случае - False</returns>
        public bool CheckHasElementsByName(string name)
        {
            return this.Elements.Any(e => e.Name == name);
        }
        #endregion

        #region method GetAttributeByName
        /// <summary>
        /// Возвращает атрибут с заданным именем
        /// </summary>
        /// <param name="name">Наименование атрибута</param>
        /// <returns>Атрибут с заданным наименованием</returns>
        public XmlAttribute GetAttributeByName(string name)
        {
            return this.Attributes.First(a => a.Name == name);
        }
        #endregion

        #region method GetAttributes
        /// <summary>
        /// Возвращает массив атрибутов элемента в том порядке, в котором они идут в документе
        /// </summary>
        /// <returns>Массив атрибутов элементов</returns>
        public XmlAttributeCollection GetAttributes()
        {
            return new XmlAttributeCollection(this.Attributes);
        }
        #endregion

        #region method GetElements
        /// <summary>
        /// Возвращает коллекцию дочерних элементов в том порядке, в котором они идут в документе
        /// </summary>
        /// <returns>Коллекция дочерних элементов</returns>
        public XmlElementCollection GetElements()
        {
            return new XmlElementCollection(this.Elements);
        }
        #endregion

        #region method GetElementByName
        /// <summary>
        /// Возвращает первый дочерний элемент с указанным расширенным именем
        /// </summary>
        /// <param name="name">Расширенное имя элемента</param>
        /// <returns>Первый дочерний элемент с указанным расширенным именем</returns>
        public XmlElement GetElementByName(string name)
        {
            return this.Elements.First(e => e.Name == name);
        }
        #endregion

        #region method GetElementsByName
        /// <summary>
        /// Возвращает массив дочерних элементов с указанным расширенным именем в том порядке, в котором они идут в документе
        /// </summary>
        /// <param name="name">Расширенное имя элемента</param>
        /// <returns>Коллекция дочерних элементов с указанным расширенным именем</returns>
        public XmlElementCollection GetElementsByName(string name)
        {
            return new XmlElementCollection(this.Elements.Where(x => x.Name == name));
        }
        #endregion
    }
}
