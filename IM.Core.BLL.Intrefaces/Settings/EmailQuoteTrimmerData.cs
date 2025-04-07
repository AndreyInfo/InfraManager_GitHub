using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Settings
{
    public class EmailQuoteTrimmerData
    {
        public Guid ID { get; init; }
        public string Name { get; init; }
        public int Sequence { get; init; }
        public string From { get; init; }
        public byte FromType { get; init; }
        public string Theme { get; init; }
        public byte ThemeType { get; init; }
        public byte[] RowVersion { get; init; }
        public string Body { get; init; }
        public byte BodyType { get; init; }
    }
}
