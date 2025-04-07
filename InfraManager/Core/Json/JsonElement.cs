using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace InfraManager.Core.Json
{
    /// <summary>
    /// Внутренний вспомогательный класс, реализующий модель контейнера для данных элемента json, поддерживающий сериализацию
    /// </summary>
    [Serializable]
    public sealed class JsonElement
    {
        #region Fields
        /// <summary>
        /// Расширенное имя элемента 
        /// </summary>
        private readonly string _name;
        /// <summary>
        /// Список дочерних элементов
        /// </summary>
        private readonly List<JsonElement> _elements;
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

        #region Elements
        /// <summary>
        /// Список дочерних элементов
        /// </summary>
        private List<JsonElement> Elements { get { return _elements; } }
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
        public JsonElement this[int index] { get { return this.Elements[index]; } }
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Название элемента</param>
        /// <param name="value">Значение</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public JsonElement(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (value == null)
                throw new ArgumentNullException("value");
            _name = name;
            _value = value;
            _elements = new List<JsonElement>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Название элемента</param>
        /// <param name="elements">Вложенные элементы</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public JsonElement(string name, IEnumerable<JsonElement> elements)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (elements == null)
                throw new ArgumentNullException("innerElements");
            _name = name;
            _value = null;
            _elements = new List<JsonElement>(elements);
        }
        #endregion

        #region private static method SkipSpaces
        private static void SkipSpaces(string json, ref int position)
        {
            while ((position < json.Length)
                    && (json[position] == ' '
                        || json[position] == '\t'
                        || json[position] == '\r'
                        || json[position] == '\n'))
                position++;
        }
        #endregion

        #region static method Parse
        /// <summary>
        /// Выполняет преобразование представления иерархии объектов из текстового формата в
        /// иерархическую объектную модель
        /// </summary>
        /// <param name="json">Содержимое Json</param>
        /// <returns>Элемент, представляющий собой иерархию объектов, описанных в json</returns>
        public static JsonElement Parse(string json)
        {
            if (json == null)
                throw new ArgumentNullException("json");
            if (string.IsNullOrEmpty(json))
                throw new ArgumentException("json");
            var position = 0;
            return Parse(json, ref position);
        }
        /// <summary>
        /// Выполняет преобразование представления иерархии объектов из текстового формата в
        /// иерархическую объектную модель
        /// </summary>
        /// <param name="json">Содержимое Json</param>
        /// <param name="position">Индекс позиции, начиная с которой необходимо проводить преобразование</param>
        /// <returns>Элемент, представляющий собой иерархию объектов, описанных в json</returns>
        private static JsonElement Parse(string json, ref int position)
        {
            return Parse(json, ref position, false);
        }
        /// <summary>
        /// Выполняет преобразование представления иерархии объектов из текстового формата в
        /// иерархическую объектную модель
        /// </summary>
        /// <param name="json">Содержимое Json</param>
        /// <param name="position">Индекс позиции, начиная с которой необходимо проводить преобразование</param>
        /// <param name="onlyValue">Флаг, указывающий, что дальше пойдут элементы без указания имени</param>
        /// <returns>Элемент, представляющий собой иерархию объектов, описанных в json</returns>
        private static JsonElement Parse(string json, ref int position, bool onlyValue)
        {
            var name = string.Empty;
            var isNamePart = !onlyValue;
            SkipSpaces(json, ref position);
            while (true)
            {
                if (json[position] == '"') //название
                    if (isNamePart)
                    {
                        var nameBeganAt = position;
                        position++;
                        while ((position < json.Length) && (json[position] != '"'))
                            position++;
                        name = json.Substring(nameBeganAt, position - nameBeganAt).Trim().Trim('"');
                    }
                    else // строковое значение
                    {
                        var valueBeganAt = position;
                        position++;
                        while (position < json.Length &&
                                (json[position] != '"' || (json[position - 1] == '\\' && json[position] == '"')))
                            position++;
                        var value = json.Substring(valueBeganAt + 1, position - valueBeganAt - 1);
                        position++;
                        return new JsonElement(name, value);
                    }
                else if (json[position] == ':') //разделитель
                    isNamePart = false;
                else if (json[position] == '{') //объект
                {
                    position++; // сдвигаемся
                    var innerElements = new List<JsonElement>();
                    while (position < json.Length)
                    {
                        var ie = Parse(json, ref position);
                        innerElements.Add(ie);
                        SkipSpaces(json, ref position);
                        if (json[position] == '}')
                            break;
                        else
                            position++;
                    }
                    position++;
                    return new JsonElement(name, innerElements);
                }
                else if (json[position] == '[') //массив
                {
                    position++; // сдвигаемся
                    var innerElements = new List<JsonElement>();
                    while ((position < json.Length))
                    {
                        var ie = Parse(json, ref position, true);
                        innerElements.Add(ie);
                        SkipSpaces(json, ref position);
                        if (json[position] == ']')
                            break;
                        else
                            position++;
                    }
                    position++;
                    return new JsonElement(name, innerElements);
                }
                else if (json[position] == ',')
                    position++;
                else //цифры или null
                {
                    var valueBeganAt = position;
                    while ((position < json.Length)
                            && (json[position] != ' '
                                && json[position] != '\t'
                                && json[position] != '\n'
                                && json[position] != '\r'
                                && json[position] != ','
                                && json[position] != ']'
                                && json[position] != '}'))
                        position++;
                    var value = json.Substring(valueBeganAt, position - valueBeganAt).Trim().Trim('"', ',');
                    return new JsonElement(name, value);
                }
                position++;
                SkipSpaces(json, ref position);
            }
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

        #region method GetElements
        /// <summary>
        /// Возвращает коллекцию дочерних элементов в том порядке, в котором они идут в документе
        /// </summary>
        /// <returns>Коллекция дочерних элементов</returns>
        public JsonElementCollection GetElements()
        {
            return new JsonElementCollection(this.Elements);
        }
        #endregion

        #region method GetElementByName
        /// <summary>
        /// Возвращает первый дочерний элемент с указанным расширенным именем
        /// </summary>
        /// <param name="name">Расширенное имя элемента</param>
        /// <returns>Первый дочерний элемент с указанным расширенным именем</returns>
        public JsonElement GetElementByName(string name)
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
        public JsonElementCollection GetElementsByName(string name)
        {
            return new JsonElementCollection(this.Elements.Where(x => x.Name == name));
        }
        #endregion
    }
}
