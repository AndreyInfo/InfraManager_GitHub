using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk
{
    public class FormValuesDataModel
    {

        public Guid FormID { get; init; }

        public DataItemModel[] Values { get; init; }
    }

    public class DataItemModel
    {
        public Guid ID { get; init; }
        public string Type { get; init; }
        public string[] Data { get; init; }
        public DataItemTableRowModel[] Rows { get; init; }
        public bool IsReadOnly { get; set; }
    }

    public class DataItemTableRowModel
    {
        public int RowNumber { get; init; }
        public DataItemModel[] Columns { get; init; }
    }
}
