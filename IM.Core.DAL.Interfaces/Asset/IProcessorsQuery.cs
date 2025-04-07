using InfraManager.DAL.Asset.Subclasses;
using InfraManager.DAL.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Asset
{
    public class ProcessorsQueryRequest
    {
        public ProcessorsQueryRequest(Guid? typeID, Guid? categoryID, ProductTemplate? templateID, string nameSearch)
        {
            TypeID = typeID;
            CategoryID = categoryID;
            TemplateID = templateID;
            NameSearch = nameSearch;
        }

        public Guid? TypeID { get; }
        public Guid? CategoryID { get; }
        public ProductTemplate? TemplateID { get; } 
        public string NameSearch { get; }
    }

    public class ProcessorsQueryResponseItem
    {
        public ProcessorsQueryResponseItem(AdapterType adapterType, Manufacturer manufacturer, Processor processor)
        {
            AdapterType = adapterType;
            Manufacturer = manufacturer;
            Processor = processor;
        }

        public AdapterType AdapterType { get; }
        public Manufacturer Manufacturer { get; }
        public Processor Processor { get; }
    }

    public interface IProcessorsListQuery
    {
        Task<List<ProcessorsQueryResponseItem>> QueryAsync(ProcessorsQueryRequest request, CancellationToken token = default);
    }
}
