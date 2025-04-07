using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.BLL.FieldEdit
{
    internal class FieldManager : IFieldManager, ISelfRegisteredService<IFieldManager>
    {
        public FieldCompareResult AreFieldsSame(object oldValue, object newValue, string fieldName)
        {
            var oldObjType = oldValue.GetType();
            var newObjType = newValue.GetType();
            if (oldObjType != newObjType)
                return FieldCompareResult.ObjectTypeDiffers;
            var propertyInfo = oldObjType.GetProperty(fieldName);
            if (propertyInfo == null)
                return FieldCompareResult.InvalidField;

            var oldFieldValue = propertyInfo.GetValue(oldValue);
            var newFieldValue = propertyInfo.GetValue(newValue);

            if (oldFieldValue!=null && oldFieldValue.Equals(newFieldValue))
                return FieldCompareResult.Equal;
            if (oldFieldValue == null && newFieldValue == null)
                return FieldCompareResult.Equal;
            return FieldCompareResult.NotEqual;
        }

        public EntityCompareAttribute GetEntityAttribute(Type objectType)
        {
            EntityCompareAttribute compareAttribute = (EntityCompareAttribute)Attribute.GetCustomAttribute(objectType, typeof(EntityCompareAttribute));
            return compareAttribute;
        }

        public FieldCompareAttribute GetFieldAttribute(Type objectType, string fieldName)
        {
            var comaprePropertyAttributes = objectType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(prop => Attribute.IsDefined(prop, typeof(FieldCompareAttribute)) && prop.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));
            return comaprePropertyAttributes.Select(x => (FieldCompareAttribute)Attribute.GetCustomAttribute(x, typeof(FieldCompareAttribute))).FirstOrDefault();
        }

        public Dictionary<string, FieldCompareAttribute> GetFieldAttributes(Type objectType)
        {
            var comaprePropertyAttributes = objectType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                                      .Where(prop => Attribute.IsDefined(prop, typeof(FieldCompareAttribute)));
            return comaprePropertyAttributes.ToDictionary(x => x.Name, x => (FieldCompareAttribute)Attribute.GetCustomAttribute(x, typeof(FieldCompareAttribute)));
        }

        public object GetFieldValue(object value, string name)
        {
            if (value == null)
                return null;
            var prop = value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(prop => Attribute.IsDefined(prop, typeof(FieldCompareAttribute)) && prop.Name == name)
                .FirstOrDefault();
            if (prop == null)
                return null;
            var fldValue = prop.GetValue(value);
            return fldValue;
        }

        public Dictionary<string, object> GetFieldsValues(object value)
        {
            if (value == null)
                return null;
            var props = value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Select(pi=> new KeyValuePair<string, object>(pi.Name, pi.GetValue(value)))
                ;
            return props.ToDictionary(x=>x.Key, y=>y.Value);
        }

        public string GetFieldValueLabel(object value, string name)
        {
            if (value == null)
                return null;
            var prop = value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(prop => Attribute.IsDefined(prop, typeof(FieldCompareAttribute)) && prop.Name == name)
                .FirstOrDefault();
            if (prop == null)
                return null;
            var fldValue = prop.GetValue(value);
            FieldCompareAttribute fieldCompare = (FieldCompareAttribute)Attribute.GetCustomAttribute(prop, typeof(FieldCompareAttribute));
            if (fldValue == null && !string.IsNullOrEmpty(fieldCompare?.NullValueLabel))
                return fieldCompare.NullValueLabel;
            if (fieldCompare != null && !string.IsNullOrEmpty(fieldCompare.Format))
                return string.Format(fieldCompare.Format, fldValue);
            if (fldValue == null)
                return string.Empty;

            if (fldValue is DateTime && (DateTime)value == DateTime.MinValue)
                return null;
            if (fldValue is bool)
                return (bool)fldValue ? "Да" : "Нет";
            return fldValue.ToString();
        }

        public BaseError? SetFieldValue(object obj, FieldValueModel fieldValue)
        {
            var objType = obj.GetType();
            var propertyItem = objType.GetProperty(fieldValue.Field);
            if (propertyItem == null)
                return BaseError.BadParamsError;

            var fieldAttribute = GetFieldAttribute(objType, fieldValue.Field);

            if (!fieldValue.ReplaceAnyway)
            {
                if (fieldAttribute.IsUseCustomValues && obj is ICustomValueManager)
                {
                    var oldValue = GetFieldValueString(propertyItem.PropertyType, fieldValue.OldValue, fieldAttribute);
                    if (!(obj as ICustomValueManager).CheckSameFieldValue(fieldValue.Field, oldValue))
                        return BaseError.ConcurrencyError;
                }
                else
                {
                    var oldValueDB = propertyItem.GetValue(obj);
                    var oldValue = GetPropertyValue(propertyItem.PropertyType, fieldValue.OldValue, fieldAttribute);
                    if (oldValue != null)
                    {
                        if (!propertyItem.PropertyType.IsArray && !oldValue.Equals(oldValueDB))
                            return BaseError.ConcurrencyError;
                        if (propertyItem.PropertyType.IsArray)
                        {
                            if (!SameArrays((oldValue as List<Dictionary<string, string>>), oldValueDB as IEnumerable<object>, GetFieldAttribute(objType, fieldValue.Field)))
                                return BaseError.ConcurrencyError;
                        }
                    }
                    if (oldValue == null && oldValueDB != null)
                        return BaseError.ConcurrencyError;
                }
            }
            if (fieldAttribute.IsUseCustomValues && obj is ICustomValueManager)
            {
                var newValue = GetFieldValueString(propertyItem.PropertyType, fieldValue.NewValue, fieldAttribute);
                (obj as ICustomValueManager).SetField(fieldValue.Field, newValue);
            }
            else
            {
                var newValue = GetPropertyValue(propertyItem.PropertyType, fieldValue.NewValue, fieldAttribute);
                if (propertyItem.PropertyType.IsArray && newValue is List<Dictionary<string, string>>)
                {
                    Type elType = propertyItem.PropertyType.GetElementType();
                    var newValues = newValue as List<Dictionary<string, string>>;
                    var newArray = Array.CreateInstance(elType, newValues.Count);
                    int id = 0;
                    foreach (var itm in newValues)
                    {
                        var newItem = Activator.CreateInstance(elType);
                        SetObjectFields(elType, newItem, itm);
                        newArray.SetValue(newItem, id++);
                    }
                    propertyItem.SetValue(obj, newArray);
                }
                else
                    propertyItem.SetValue(obj, newValue);
            }

            return null;
        }

        private void SetObjectFields(Type elType, object newItem, Dictionary<string, string> values)
        {
            foreach(var fld in elType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (values.ContainsKey(fld.Name))
                    fld.SetValue(newItem, GetValue(values[fld.Name], fld.PropertyType));
            }
        }

        private bool SameArrays(List<Dictionary<string,string>> oldValue, IEnumerable<object> oldValueDB, FieldCompareAttribute fieldCompareAttribute)
        {
            if (oldValue == null || oldValueDB == null)
                return false;
            if (oldValue.Count() != oldValueDB.Count())
                return false;
            bool areSame = true;
            int id = 0;
            foreach(var oldItem in oldValue)
            {
                object dbItem = null;
                if (!string.IsNullOrEmpty(fieldCompareAttribute.ListIDField))
                {
                    var idValue = oldItem.ContainsKey(fieldCompareAttribute.ListIDField)?oldItem[fieldCompareAttribute.ListIDField] : string.Empty;
                    dbItem = oldValueDB.FirstOrDefault(x => GetFieldValue(x, fieldCompareAttribute.ListIDField).ToString().Equals(idValue));
                }
                else
                {
                    dbItem = oldValueDB.Skip(id++).First();
                }
                if (dbItem == null)
                {
                    areSame = false;
                    break;
                }
                if(!SameAttributes(oldItem, dbItem))
                {
                    areSame = false;
                    break;
                }
            }
            return areSame;
        }

        private bool SameAttributes(Dictionary<string,string> oldItem, object dbItem)
        {
            Dictionary<string, object> dbItemAttributes = GetFieldsValues(dbItem);

            if (oldItem == null || dbItemAttributes == null || oldItem.Count != dbItemAttributes.Count)
                return false;
            return oldItem.All(v => dbItemAttributes.Any(d => d.Key == v.Key && ((d.Value != null && !string.IsNullOrEmpty(v.Value) && d.Value.Equals(GetValue(v.Value, d.Value.GetType()))) || (string.IsNullOrEmpty(v.Value) && d.Value == null))));
        }

        private object GetPropertyValue(Type propertyType, string valueFromSite, FieldCompareAttribute fieldAttribute)
        {
            if (string.IsNullOrEmpty(valueFromSite))
                return null;
            if (propertyType.IsArray)
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(valueFromSite);
                return data;
            }
            else
            {
                var stringValue = GetFieldValueString(propertyType, valueFromSite, fieldAttribute);
                if(stringValue!=null)
                    return GetValue(stringValue, propertyType);
            }
            return null;
        }

        private string GetFieldValueString(Type propertyType, string valueFromSite, FieldCompareAttribute fieldAttribute)
        {
            string deserializedValueName = fieldAttribute?.SetFieldProperty ?? string.Empty;
            if (string.IsNullOrWhiteSpace(deserializedValueName))
            {
                if (propertyType.IsAssignableFrom(typeof(string)))
                    deserializedValueName = "text";
                else if (propertyType.IsAssignableFrom(typeof(int))
                    || propertyType.IsAssignableFrom(typeof(byte))
                    || propertyType.IsAssignableFrom(typeof(Guid))
                    || propertyType.IsEnum)
                    deserializedValueName = "id";
                else
                    deserializedValueName = "text";
            }

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(valueFromSite);
            if (data.ContainsKey(deserializedValueName))
            {
                return data[deserializedValueName];
            }
            return null;
        }

        private object GetValue(string sValue, Type type)
        {
            if (type.IsAssignableFrom(typeof(bool)) && bool.TryParse(sValue, out var boolValue))
                return boolValue;
            if (type.IsAssignableFrom(typeof(byte)) && byte.TryParse(sValue, out var byteValue))
                return byteValue;
            if (type.IsAssignableFrom(typeof(int)) && int.TryParse(sValue, out var intValue))
                return intValue;
            if (type.IsAssignableFrom(typeof(decimal)) && decimal.TryParse(sValue, out var decimalValue))
                return decimalValue;
            if (type.IsAssignableFrom(typeof(decimal)) && decimal.TryParse(sValue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var decimalValue1))
                return decimalValue1;
            if (type.IsAssignableFrom(typeof(Guid)) && Guid.TryParse(sValue, out var guidValue))
                return guidValue;
            if (type.IsEnum)
            {
                if (Enum.TryParse(type, sValue, out var result))
                    return result;
            }
            if (type.IsAssignableFrom(typeof(string)))
                return sValue;
            return null;
        }
    }
}
