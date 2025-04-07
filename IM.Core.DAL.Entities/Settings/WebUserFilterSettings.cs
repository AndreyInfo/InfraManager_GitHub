using System;
using System.Linq;

namespace InfraManager.DAL.Settings
{
    public class WebUserFilterSettings
    {
        public WebUserFilterSettings(
            Guid userId, 
            string viewName, 
            WebFilter filter = null, 
            bool temp = false, 
            bool withFinishedWorkflow = false, 
            DateTime? afterUtcDateModified = null)
        {
            UserId = userId;
            ViewName = viewName;
            Filter = filter;
            Temp = temp;
            WithFinishedWorkflow = withFinishedWorkflow;
            AfterUtcDateModified = afterUtcDateModified;
        }

        protected WebUserFilterSettings()
        {
        }

        public Guid UserId { get; private set; }
        public string ViewName { get; private set; }
        public virtual WebFilter Filter { get; set; }
        public bool Temp { get; set; }
        public bool WithFinishedWorkflow { get; set; }
        public DateTime? AfterUtcDateModified { get; set; }
        public FilterElementData[] GetElements()
        {
            return Filter?.GetElements() ?? Array.Empty<FilterElementData>();
        }
    }
}
