using System;

namespace InfraManager.BLL
{
    public class ListFilter : ListFilterBase
    {
        public bool WithFinishedWorkflow { get; set; }
        public string AfterModifiedMilliseconds { get; set; }
        public ListTreeFilter TreeSettings { get; set; }
        public Guid? ParentID { get; set; }
        public int? ParentClassID { get; set; }
        public SoftwarePoolFilter SoftwarePoolSettings { get; set; }
        public Guid? ID { get; set; }
        public Guid[] IDList { get; set; }
    }
}
