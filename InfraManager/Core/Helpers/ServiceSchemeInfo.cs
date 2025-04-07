using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.Core.Helpers
{
    public sealed class SchemeNodeInfo
    {
        public Double PositionX { get; set; }
        public Double PositionY { get; set; }
        public Double Width { get; set; }
        public Double Height { get; set; }
        //public string ID { get; set; }
        //public string Name { get; set; }
        //public string ClassName { get; set; }
        public Guid ObjectID { get; set; }
        public int ObjectClassID { get; set; }
        //public List<NodeFieldInfo> NodeFieldInfoList { get; set; }
        public object NodeObject { get; set; }
        public string IconClass { get; set; }
    }

    public sealed class NodeFieldInfo
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }

    public sealed class SchemeConnectionInfo
    {
        public string SourceID { get; set; }
        public string TargetID { get; set; }
    }
}
