using System;

namespace InfraManager.BLL.ProductCatalogue.Synonyms
{
    public class SynonymFilter
    {
        public Guid? AdapterProductCatalogTypeID { get; set; }
        
        public Guid? PeripheralProductCatalogTypeID { get; set; }

        public Guid? ModelID { get; set; }
        
        public string WithoutModelName { get; set; }
        
        public string WithoutModelProducer { get; set; }

        public string ModelName { get; init; }

        public string ModelProducer { get; init; }

        public ObjectClass? ClassID { get; init; }
    }
}