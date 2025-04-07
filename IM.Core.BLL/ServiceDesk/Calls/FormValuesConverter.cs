using System;
using AutoMapper;
using InfraManager.BLL.ServiceDesk.CustomValues;
using InfraManager.BLL.ServiceDesk.FormDataValue;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;
using System.Collections.Generic;
using System.Linq;
using InfraManager.DAL;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class FormValuesDataConverter : ITypeConverter<FormValuesData, FormValues>
    {
        public FormValues Convert(FormValuesData source, FormValues destination, ResolutionContext context)
        {
            if (source?.Values is null)
            {
                return default;
            }

            var valuesFromSource = FilterValues(source.Values)
                // значения НЕтабличных параметров
                .SelectMany(x => x?.Data?.Select((m, i) => BuildValue(m, x.ID, i, 0, x.IsReadOnly)))
                // значения табличных параметров
                .Concat(GetTableValues(source.Values))
                .ToList();

            if (destination is null)
            {
                return new FormValues
                {
                    FormBuilderFormID = source.FormID,
                    Values = valuesFromSource
                };
            }

            if (source.FormID != destination.FormBuilderFormID) {
                return destination;
            }

            destination.Values.Clear();
            destination.Values = valuesFromSource;
            
            return destination;
        }

        private static IEnumerable<Values> GetTableValues(IEnumerable<DataItem> values)
        {
            return values
                .Where(v => v.Type == "Table" && v.Rows != null && v.Rows.Any())
                .SelectMany(item => item.Rows)
                .Where(row => row.Columns != null && row.Columns.Any())
                .SelectMany(
                    (row, _) => row.Columns,
                    (row, column) => new
                    {
                        RowNumber = row.RowNumber,
                        Column = column,
                    })
                .Where(item => item.Column.Data != null && item.Column.Data.Any())
                .SelectMany(item => item.Column.Data.Select(
                    (data, index) => BuildValue(data, item.Column.ID, index, item.RowNumber, item.Column.IsReadOnly))
                );
        }

        private static Values BuildValue(string value, Guid formFieldID, int order, int rowNumber, bool isReadOnly)
        {
            return new Values
            {
                Value = value,
                FormFieldID = formFieldID,
                Order = order,
                RowNumber = rowNumber,
                IsReadOnly = isReadOnly,
            };
        }

        private static IEnumerable<DataItem> FilterValues(IEnumerable<DataItem> values)
            => values.Where(v => v.Data is not null && (v.Type is null ||
                                                        (v.Type != "Group" &&
                                                         v.Type != "Header" &&
                                                         v.Type != "Separator" &&
                                                         v.Type != "Table")));
    }

    public class FormValuesConverter : ITypeConverter<FormValues, FormValuesDetailsModel>
    {
        private readonly CustomValueFactory _customValueFactory;
        private readonly IReadonlyRepository<Form> _forms;

        public FormValuesConverter(
            CustomValueFactory customValueFactory,
            IReadonlyRepository<Form> forms)
        {
            _customValueFactory = customValueFactory;
            _forms = forms;
        }

        public FormValuesDetailsModel Convert(FormValues data, FormValuesDetailsModel destination, ResolutionContext context)
        {
            if (data?.Values is null)
            {
                return default;
            }

            var allTableFields = GetFormOrLoad(data.Form, data.FormBuilderFormID).FormTabs
                .SelectMany(tab => tab.Fields)
                .Where(field => field.Type == FieldTypes.Table)
                .GroupBy(field => field.ID)
                .ToDictionary(grouping => grouping.Key, g => g.First());

            var allValues = data.Values.Where(v => v.FormField?.Type != null).ToArray();

            var formValues = allValues
                    .Where(value => !value.FormField.ColumnFieldID.HasValue)
                    .GroupBy(value => value.FormFieldID)
                    .Select(group => new
                    {
                        FormID = group.Key,
                        IsReadOnly = group.FirstOrDefault()?.IsReadOnly ?? false,
                        Values = group.ToArray(),
                    })
                    .Select(item => CreateFormValue(item.FormID, item.Values))
                    .Concat(allValues
                        .Where(v => v.FormField.ColumnFieldID.HasValue)
                        .GroupBy(v => v.FormField.ColumnFieldID.Value)
                        .Select(tableValues => CreateTableFormValue(allTableFields[tableValues.Key], tableValues)))
                    .ToArray();

            return new FormValuesDetailsModel
            {
                Values = formValues,
                FormValuesID = data.ID,
                FormID = data.FormBuilderFormID
            };
        }

        private FormValue CreateTableFormValue(FormField tableField, IEnumerable<Values> tableValues)
        {
            var rows = tableValues
                .GroupBy(values => values.RowNumber)
                .OrderBy(values => values.Key)
                .Select(rowValues =>
                    new TableRow
                    {
                        RowNumber = rowValues.Key,
                        Columns = rowValues
                            .GroupBy(column => column.FormFieldID)
                            .Select(group => new
                            {
                                FieldID = group.Key,
                                Values = group.ToArray(),
                                IsReadOnly = group.FirstOrDefault()?.IsReadOnly ?? false,
                            })
                            .Select(item => CreateFormValue(item.FieldID, item.Values))
                            .OrderBy(column => column.Order)
                            .ToArray(),
                    });

            return new FormValue
            {
                ID = tableField.ID,
                Type = tableField.Type,
                Identifier = tableField.Identifier,
                Order = tableField.Order,
                Rows = rows.ToArray(),
            };
        }

        private FormValue CreateFormValue(Guid fieldID, Values[] values)
        {
            return new FormValue
            {
                ID = fieldID,
                Identifier = values[0].FormField.Identifier,
                Type = values[0].FormField.Type,
                Order = values[0].FormField.Order,
                Data = values.OrderBy(x => x.Order)
                    .Select(x => CreateItem(x.Value, x.FormField, x.Order))
                    .ToArray(),
                IsReadOnly = values[0].IsReadOnly,
            };
        }

        private ItemValue CreateItem(string val, FormField field, int order)
        {
            var itemValue = val switch
            {
                null => new ItemValue(),
                _ => _customValueFactory.GetCustomType(field).GetValue(val, order),
            };

            itemValue.Order = order;

            return itemValue;
        }

        private Form GetFormOrLoad(Form form, Guid formID)
        {
            return form ?? _forms.WithMany(f => f.FormTabs)
                .ThenWithMany(tab => tab.Fields)
                .First(f => f.ID == formID);
        }
    }

}
