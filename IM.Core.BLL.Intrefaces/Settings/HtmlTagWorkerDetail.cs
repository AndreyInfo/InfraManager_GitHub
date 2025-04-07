using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public class HtmlTagWorkerDetail
    {
        public Guid ID { get; init; }
        public Guid QuoteTrimmerID { get; init; }
        public string Name { get; init; }
        public int Sequence { get; init; }
        public string TagName { get; init; }
        public string Text { get; init; }
        public SearchType? TextType { get; init; }
        public string Class { get; init; }
        public SearchType? ClassType { get; init; }
        public string Style { get; init; }
        public SearchType? StyleType { get; init; }
        public string AttrName { get; init; }
        public string AttrValue { get; init; }
        public SearchType? AttrType { get; init; }
        public Guid? LeftID { get; init; }
        public Guid? RightID { get; init; }
        public Guid? InnerID { get; init; }
        public bool Repeat { get; init; }
        public bool Skip { get; init; }
        public bool InnerNot { get; init; }
        public bool RightNot { get; init; }
        public bool LeftNot { get; init; }
        public bool OnlyMe { get; init; }
        public byte[] RowVersion { get; init; }
    }
}
