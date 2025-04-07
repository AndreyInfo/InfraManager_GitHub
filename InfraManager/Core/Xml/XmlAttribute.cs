using System;
using System.Xml.Linq;

namespace InfraManager.Core.Xml
{
    /// <summary>
    /// Класс, реализующий модель контейнера для данных атрибута xml, поддерживающий сериализацию
    /// </summary>
    [Serializable]
    public sealed class XmlAttribute
    {
        #region Fields
        /// <summary>
        /// Наименование атрибута
        /// </summary>
        private readonly string _name;
        /// <summary>
        /// Значение атрибута
        /// </summary>
        private readonly string _value;
        #endregion

        #region Properties
        #region Name
        /// <summary>
        /// Наименование атрибута
        /// </summary>
        public string Name { get { return _name; } }
        #endregion

        #region Value
        /// <summary>
        /// Значение атрибута
        /// </summary>
        public string Value { get { return _value; } }
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xAttribute">Атрибут, который необходимо преобразовать</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public XmlAttribute(XAttribute xAttribute)
        {
            if (xAttribute == null)
                throw new ArgumentNullException("xAttribute");
            _name = xAttribute.Name.ToString();
            _value = xAttribute.Value;
        }
        #endregion
    }
}
