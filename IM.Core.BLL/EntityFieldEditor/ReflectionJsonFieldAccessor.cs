using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.EntityFieldEditor
{
    /// <summary>
    /// Обрабатывает полученные значения объекта  виде строки, содержащей Json объект
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ReflectionJsonArrayFieldAccessor<TEntity, TItem> : IFieldAccessor<TEntity, SetFieldData>
        where TEntity : class
    {
        private readonly string _fieldName;
        private readonly IEqualityComparer<TItem> _equalityComparer;
        private readonly Func<TItem, object> _sortExpression;

        public ReflectionJsonArrayFieldAccessor(string fieldName, IEqualityComparer<TItem> equalityComparer, Func<TItem, object> sortExpression)
        {
            _fieldName = fieldName;
            _equalityComparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));
            _sortExpression = sortExpression ?? throw new ArgumentNullException(nameof(sortExpression));
        }

        public object GetValue(TEntity entity)
        {
            return typeof(TEntity).GetProperty(_fieldName).GetValue(entity);
        }

        public bool MatchesCurrent(TEntity entity, SetFieldData data)
        {
            try
            {
                var currentValue = typeof(TEntity).GetProperty(_fieldName).GetValue(entity);
                if (data.OldValue == null)
                    return currentValue == null;
                if (data.OldValue.GetType() != typeof(string))
                    return false;

                var oldValue = Newtonsoft.Json.JsonConvert.DeserializeObject(data.OldValue.ToString(), currentValue.GetType());

                if (oldValue == null)
                    return currentValue == null;

                return (currentValue as IEnumerable<TItem>).OrderBy(_sortExpression).SequenceEqual((oldValue as IEnumerable<TItem>).OrderBy(_sortExpression), _equalityComparer);
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
                if (data.NewValue.GetType() != typeof(string))
                    return BaseError.BadParamsError;
                newValue = Newtonsoft.Json.JsonConvert.DeserializeObject(data.NewValue.ToString(), fieldProperty.PropertyType);

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
