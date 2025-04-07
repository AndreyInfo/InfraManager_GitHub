using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using InfraManager.DAL.FormBuilder;
using InfraManager.Linq;

namespace InfraManager.BLL.FormBuilder;

public static class FormExtensions
{
    private const double C_lastIndex = 0.0001d;
    
    public static Form Clone(this Form form, string name, string description, string identifier, int majorVersion = 0,
        int minorVersion = 1, double lastIndex = C_lastIndex, Guid? mainID = null)
    {
        var clonedForm = new Form
        {
            Description = description,
            Name = name,
            Identifier = identifier,
            ClassID = form.ClassID,
            Width = form.Width,
            Height = form.Height,
            FieldsIsRequired = form.FieldsIsRequired,
            FormTabs = form.FormTabs,
            MajorVersion = majorVersion,
            MinorVersion = minorVersion,
            MainID = mainID ?? Guid.Empty,
            LastIndex = lastIndex
        };

        foreach (var tab in clonedForm.FormTabs)
        {
            tab.ID = Guid.Empty;
            tab.FormID = Guid.Empty;
            foreach (var field in tab.Fields)
            {
                if (field.Grouped != null && field.Grouped.Any())
                {
                    ClearFields(field.Grouped, x => x.GroupFieldID);
                }

                if (field.Columns != null && field.Columns.Any())
                {
                    ClearFields(field.Columns, x => x.ColumnFieldID);
                }

                field.ID = Guid.Empty;
                field.TabID = Guid.Empty;
                if (field.Options != null)
                {
                    foreach (var option in field.Options)
                    {
                        option.WorkflowActivityFormFieldID = Guid.Empty;
                        option.ID = Guid.Empty;
                    }
                }
            }
        }

        return clonedForm;
    }

    private static void ClearFields(ICollection<FormField> fields, Expression<Func<FormField, Guid?>> clearProperty)
    {
        foreach (var el in fields)
        {
            el.ID = Guid.Empty;
            el.TabID = Guid.Empty;
            ExpressionExtensions.ChangeValueWithExpression(el, Guid.Empty, clearProperty);
            
            if (el.Options != null)
            {
                foreach (var option in el.Options)
                {
                    option.WorkflowActivityFormFieldID = Guid.Empty;
                    option.ID = Guid.Empty;
                }
            }
        }
    }
}