using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.Settings
{
    public class WebFilter
    {
        public WebFilter()
        {
            Elements = new HashSet<WebFilterElement>();
        }

        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Name { get; set; }
        public bool Standart { get; set; }
        public string ViewName { get; set; }
        public string Description { get; set; }
        public bool? Others { get; set; }
        public virtual ICollection<WebFilterElement> Elements { get; set; }
        public FilterElementData[] GetElements()
        {
            return Elements.Select(x => x.Parse()).ToArray()
                ?? Array.Empty<FilterElementData>();
        }
    }
}