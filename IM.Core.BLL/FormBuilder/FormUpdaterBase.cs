using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.FormBuilder.Contracts;
using InfraManager.DAL;
using InfraManager.DAL.ChangeTracking;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk;
using Newtonsoft.Json;

namespace InfraManager.BLL.FormBuilder;

/// <summary>
/// Реализует логику обновления доп. параметров объекта.
/// </summary>
/// <typeparam name="TEntity">Тип объекта</typeparam>
internal abstract class FormUpdaterBase<TEntity>
    where TEntity : class, IHaveFormValues
{
    private readonly IReadonlyRepository<Subdivision> _subdivisions;
    private readonly IReadonlyRepository<User> _users;

    protected FormUpdaterBase(
        IReadonlyRepository<Subdivision> subdivisions,
        IReadonlyRepository<User> users)
    {
        _subdivisions = subdivisions;
        _users = users;
    }

    public void Visit(IEntityState originalState, TEntity currentState)
    {
        if (ShouldRecalculate(originalState, currentState))
        {
            UpdateFormAsync(currentState, GetTemplate(currentState), CancellationToken.None).GetAwaiter().GetResult();
        }
    }

    public async Task VisitAsync(IEntityState originalState, TEntity currentState, CancellationToken cancellationToken)
    {
        if (await ShouldRecalculateAsync(originalState, currentState, cancellationToken))
        {
            await UpdateFormAsync(currentState, await GetTemplateAsync(currentState, cancellationToken), cancellationToken);
        }
    }

    protected abstract bool ShouldRecalculate(IEntityState originalState, TEntity currentState);
    protected abstract Task<bool> ShouldRecalculateAsync(IEntityState originalState, TEntity currentState, CancellationToken cancellationToken);
    protected abstract FormBuilderFullFormDetails GetTemplate(TEntity currentState);
    protected abstract Task<FormBuilderFullFormDetails> GetTemplateAsync(TEntity currentState, CancellationToken cancellationToken);

    private async Task UpdateFormAsync(TEntity currentState, FormBuilderFullFormDetails template, CancellationToken cancellationToken)
    {
        if (template is null || template.Form.ID == Guid.Empty)
        {
            currentState.FormValues.Values.Clear();
            currentState.FormValues = default;
            currentState.FormValuesID = null;
            currentState.FormID = null;

            return;
        }

        currentState.FormValues ??= new FormValues { FormBuilderFormID = template.Form.ID, };

        var fields = template.Elements.SelectMany(tab => tab.TabElements).ToArray();
        var values = currentState.FormValues.Values
            .Where(val => val.Order == 0) // NOTE: Для полей с флагом "Несколько значений" можем взять значение только из первого поля
            .ToDictionary(val => (val.FormField.Identifier, val.FormField.Type));

        if (currentState.FormID != template.Form.ID)
        {
            currentState.FormID = template.Form.ID;
            currentState.FormValues = new FormValues
            {
                FormBuilderFormID = template.Form.ID,
            };
        }

        currentState.FormValues.Values.Clear();
        foreach (var field in fields)
        {
            var value = await TryGetValueAsync(field, values, cancellationToken);
            currentState.FormValues.Values.Add(new Values { FormFieldID = field.ID, Value = value ?? string.Empty, });
        }
    }

    private async Task<string> TryGetValueAsync(
        FormBuilderFormTabFieldDetails field,
        IReadOnlyDictionary<(string, FieldTypes), Values> keyValues,
        CancellationToken cancellationToken)
    {
        return keyValues.TryGetValue((field.Identifier, field.Type), out var value)
            ? await MeasureFieldValueAsync(field, value.Value, cancellationToken)
            : default;
    }

    private async Task<string> MeasureFieldValueAsync(FormBuilderFormTabFieldDetails field, string value, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }

        var specification = (FormBuilderFieldSpecialFields) JsonConvert.DeserializeObject<FormBuilderFieldSpecialFields>(Convert.ToString(field.SpecialFields));
        return field.Type switch
        {
            FieldTypes.Boolean
                or FieldTypes.DateTime
                or FieldTypes.Position
                or FieldTypes.Service => value,

            FieldTypes.Number =>
                (specification.MinValue.HasValue
                 && specification.MaxValue.HasValue
                 && decimal.TryParse(value, out var result)
                 && result >= specification.MinValue.Value
                 && result <= specification.MaxValue.Value)
                || (!specification.MinValue.HasValue && !specification.MaxValue.HasValue)
                    ? value
                    : default,

            FieldTypes.TextArea
                or FieldTypes.String
                or FieldTypes.Password =>
                (specification.MinValue.HasValue
                 && specification.MaxValue.HasValue
                 && value.Length >= specification.MinValue.Value
                 && value.Length <= specification.MaxValue.Value)
                || (!specification.MinValue.HasValue && !specification.MaxValue.HasValue)
                    ? value
                    : default,

            FieldTypes.Subdivision => await GetSubdivisionFieldValueOrNullAsync(specification, value, cancellationToken),

            FieldTypes.User => await GetUserFieldValueOrNullAsync(specification, value, cancellationToken),

            FieldTypes.EnumComboBox
                or FieldTypes.EnumRadioButton => GetListValueOrNull(specification, value),

            FieldTypes.Table => throw new NotSupportedException($"Parameter of type '{field.Type}' not supported."),

            _ => throw new ArgumentOutOfRangeException(nameof(FormBuilderFormTabFieldDetails.Type), field.Type, $"Unknown field type value '{field.Type}'."),
        };
    }

    private async Task<string> GetSubdivisionFieldValueOrNullAsync(FormBuilderFieldSpecialFields fieldSpecification, string value, CancellationToken cancellationToken)
    {
        var currentSubdivisionID = Guid.Parse(value);
        if (!(fieldSpecification.SubdivisionID.HasValue || fieldSpecification.OrganizationID.HasValue)
            || fieldSpecification.SubdivisionID.HasValue && fieldSpecification.SubdivisionID.Value == currentSubdivisionID)
        {
            return value;
        }

        var subdivisions = await _subdivisions.DisableTrackingForQuery()
            .ToArrayAsync(x => x.ID == fieldSpecification.SubdivisionID || x.OrganizationID == fieldSpecification.OrganizationID, cancellationToken);

        if (!subdivisions.Any())
        {
            return default;
        }

        var nodes = new Queue<Subdivision>(subdivisions);
        while (nodes.TryDequeue(out var current))
        {
            var currentID = current.ID;
            if (currentID == currentSubdivisionID)
            {
                return value;
            }

            // NOTE: WithMany не работает на всю глубину.
            foreach (var child in await _subdivisions.DisableTrackingForQuery()
                         .ToArrayAsync(x => x.SubdivisionID == currentID, cancellationToken))
            {
                nodes.Enqueue(child);
            }
        }

        return default;
    }

    private async Task<string> GetUserFieldValueOrNullAsync(FormBuilderFieldSpecialFields fieldSpecification, string value, CancellationToken cancellationToken)
    {
        var currentUserID = Guid.Parse(value);
        if (!fieldSpecification.SubdivisionID.HasValue)
        {
            return value;
        }

        Subdivision current = null;
        var nodes = new Queue<Subdivision>();
        do
        {
            var subdivisionID = current?.ID ?? fieldSpecification.SubdivisionID.Value;
            if (await _users.AnyAsync(x => x.IMObjID == currentUserID && x.SubdivisionID == subdivisionID, cancellationToken))
            {
                return value;
            }

            foreach (var child in await _subdivisions.DisableTrackingForQuery()
                         .ToArrayAsync(x => x.SubdivisionID == subdivisionID, cancellationToken))
            {
                nodes.Enqueue(child);
            }
        } while (nodes.TryDequeue(out current));

        return default;
    }

    private static string GetListValueOrNull(FormBuilderFieldSpecialFields fieldSpecification, string value)
    {
        if (fieldSpecification == null || !fieldSpecification.List.Any())
        {
            return default;
        }

        var currentID = Guid.Parse(value);
        return fieldSpecification.List.Any(x => x.ID == currentID) ? value : default;
    }
}