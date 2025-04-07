using Newtonsoft.Json;
using System;

namespace InfraManager.DAL.Settings
{
    public class WebFilterElement
    {
        protected WebFilterElement()
        {
        }

        public WebFilterElement(FilterElementData data)
        {
            Id = Guid.NewGuid();
            Data = JsonConvert.SerializeObject(data);
        }

        public Guid Id { get; init; }
        public string Data { get; private set; }

        public FilterElementData Parse()
        {
            return JsonConvert.DeserializeObject<FilterElementData>(Data);
        }
    }
}