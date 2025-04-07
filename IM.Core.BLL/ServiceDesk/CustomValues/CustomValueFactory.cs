using InfraManager.DAL;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.Parameters;
using System;
using System.Collections.Generic;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceDesk.CustomValues
{
    public class CustomValueFactory
    {
        private readonly Dictionary<FieldTypes, Func<FormField, IGetValue>> _types = new ();

        public CustomValueFactory(
            IRepository<User> users,
            IRepository<JobTitle> jobTitles,
            IRepository<ParameterEnumValue> parameterEnumValues,
            IRepository<Subdivision> subdivisions,
            IFinder<Service> serviceFinder)
        {
            _types.Add(FieldTypes.User, field => new UserType(users));
            _types.Add(FieldTypes.Subdivision, field => new SubdivisionType(subdivisions));
            _types.Add(FieldTypes.Position, field => new PositionType(jobTitles));
            _types.Add(FieldTypes.EnumComboBox, field => new EnumType(parameterEnumValues));
            _types.Add(FieldTypes.EnumRadioButton, field => new EnumType(parameterEnumValues));
            _types.Add(FieldTypes.Service, field => new ServiceType(serviceFinder));
        }

        public IGetValue GetCustomType(FormField field)
        {
            var type = field?.Type ?? throw new ArgumentNullException(nameof(field.Type));
            return _types.TryGetValue(type, out var factory) ? factory(field) : new DefaultType();
        }
    }
}
