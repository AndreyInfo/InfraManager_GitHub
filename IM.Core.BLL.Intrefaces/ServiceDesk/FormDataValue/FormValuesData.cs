using System;

namespace InfraManager.BLL.ServiceDesk.FormDataValue
{
    public class FormValuesData
    {

        public Guid FormID { get; init; }

        public DataItem[] Values { get; init; }
    }

    public class DataItem
    {
        public Guid ID { get; init; }

        public string Type { get; init; }

        public string[] Data { get; init; }
        
        /// <summary>
        /// Возвращает строки параметра с типом Таблица.
        /// </summary>
        public DataItemTableRow[] Rows { get; init; }

        public bool IsReadOnly { get; init; }
    }

    /// <summary>
    /// Представляет одну строку параметра с типом таблица.
    /// </summary>
    public class DataItemTableRow
    {
        /// <summary>
        /// Возвращает номер строки.
        /// </summary>
        public int RowNumber { get; init; }

        /// <summary>
        /// Возвращает колонки таблицы в этой строке.
        /// </summary>
        public DataItem[] Columns { get; init; }
    }
}
