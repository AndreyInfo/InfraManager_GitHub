using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace InfraManager.DAL.ChangeTracking
{
    internal class PropertyValuesWrapper : IEntityState
    {
        private readonly PropertyValues _values;
        private readonly Dictionary<string, object> _references;

        public PropertyValuesWrapper(
            PropertyValues values,
            Dictionary<string, object> references)
        {
            _values = values;
            _references = references;
        }

        public object this[string property] => _values[property];

        public object Reference(string name)
        {
            return _references[name];
        }
    }
}
