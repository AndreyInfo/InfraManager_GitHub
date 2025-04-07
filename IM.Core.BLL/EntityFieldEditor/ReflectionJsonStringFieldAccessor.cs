using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.EntityFieldEditor
{
    /// <summary>
    /// Обрабатывает полученные значения объекта  виде строки, содержащей простой Json объект с заданным именем поля:
    /// {"val":"false"}
    /// {"text":"text"}
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ReflectionJsonStringFieldAccessor<TEntity> : IFieldAccessor<TEntity, SetFieldData>
        where TEntity : class
    {
        private readonly string _fieldName;
        private readonly string _jsonFieldName;

        public ReflectionJsonStringFieldAccessor(string fieldName, string jsonFieldName = null)
        {
            _fieldName = fieldName;
            _jsonFieldName = jsonFieldName;
        }

        public object GetValue(TEntity entity)
        {
            return typeof(TEntity).GetProperty(_fieldName).GetValue(entity);
        }

        public bool MatchesCurrent(TEntity entity, SetFieldData data)
        {
            try
            {
                var fieldProperty = typeof(TEntity).GetProperty(_fieldName);
                var currentValue = fieldProperty.GetValue(entity);
                if (data.OldValue == null)
                    return currentValue == null;
                if (data.OldValue.GetType() != typeof(string))
                    return false;

                var oldRes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(data.OldValue.ToString());
                var oldValueStr = oldRes!=null && !string.IsNullOrWhiteSpace(_jsonFieldName) && oldRes.ContainsKey(_jsonFieldName) 
                    ? oldRes[_jsonFieldName] 
                    : oldRes!=null && string.IsNullOrWhiteSpace(_jsonFieldName) && oldRes.Any() ? oldRes.First().Value : null;

                var propertyType = fieldProperty.PropertyType;
                var isNullableType = Nullable.GetUnderlyingType(propertyType) != null;
                if (isNullableType)
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                var oldValue = fieldProperty.PropertyType == typeof(string) ? oldValueStr : propertyType.GetMethod("Parse", new System.Type[] { typeof(string) })?.Invoke(null, new object[] { oldValueStr });

                if (oldValue == null)
                    return currentValue == null;

                if (isNullableType &&
                    oldValue is int && (int)oldValue == default(int))
                    return currentValue == default;

                return oldValue.Equals(currentValue);
            }
            catch
            {
                return false;
            }
        }

        public BaseError SetValue(TEntity entity, SetFieldData data)
        {
            try
            {
                var fieldProperty = typeof(TEntity).GetProperty(_fieldName);
                object newValue = null;
                if (data.NewValue!=null)
                {
                    if (data.NewValue.GetType() != typeof(string))
                        return BaseError.BadParamsError;
                    var newRes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(data.NewValue.ToString());
                    var newValueStr = newRes != null && !string.IsNullOrWhiteSpace(_jsonFieldName) && newRes.ContainsKey(_jsonFieldName) 
                        ? newRes[_jsonFieldName] 
                        : newRes != null && string.IsNullOrWhiteSpace(_jsonFieldName) && newRes.Any() ? newRes.First().Value : null;

                    var propertyType = fieldProperty.PropertyType;
                    var isNullableType = Nullable.GetUnderlyingType(propertyType) != null;
                    if (isNullableType)
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyType);
                    }
                    newValue = fieldProperty.PropertyType == typeof(string) ? newValueStr : propertyType.GetMethod("Parse", new System.Type[] { typeof(string) })?.Invoke(null, new object[] { newValueStr });
                }

                fieldProperty.SetValue(entity, newValue);
                return BaseError.Success;
            }
            catch
            {
                return BaseError.BadParamsError;
            }
        }
    }
}
