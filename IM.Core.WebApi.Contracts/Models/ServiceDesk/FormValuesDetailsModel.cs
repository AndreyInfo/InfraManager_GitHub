using System;
using InfraManager.DAL.FormBuilder;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk
{
    [Serializable]
    public class FormValuesDetailsModel
    {
        public long FormValuesID { get; init; }

        public FormValue[] Values { get; init; }
        public Guid FormID { get; init; }
    }

    [Serializable]
    public class FormValue
    {
        public Guid ID { get; init; }

        public string Name { get; init; }

        public string Identifier { get; init; }

        public int Order { get; init; }

        public FieldTypes Type { get; init; }

        public ItemValue[] Data { get; init; }
        
        /// <summary>
        /// Возвращает массив строк параметра с типом Таблица.
        /// </summary>
        public TableRow[] Rows { get; init; }

        public bool IsReadOnly { get; init; }
    }

    /// <summary>
    /// Представляет одну строку параметра с типом Таблица.
    /// </summary>
    [Serializable]
    public class TableRow
    {
        /// <summary>
        /// Возвращает номер строки в таблице.
        /// </summary>
        public int RowNumber { get; init; }
        
        /// <summary>
        /// Возвращает колонки в этой строке таблицы.
        /// </summary>
        public FormValue[] Columns { get; init; }
    }
    
    [Serializable]
    public class ItemValue
    {
        public string ValueID { get; init; } = string.Empty;

        public string Value { get; init; }

        public int Order { get; set; }
    }
}
