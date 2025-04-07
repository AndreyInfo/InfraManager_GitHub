using System;

namespace InfraManager.BLL.ProductCatalogue.Synonyms
{
    public class SynonymDetails
    {
        public ObjectClass ClassID { get; init; }

        public Guid ModelID { get; init; }

        public string ModelName { get; init; }

        public string ModelProducer { get; init; }
    }
}