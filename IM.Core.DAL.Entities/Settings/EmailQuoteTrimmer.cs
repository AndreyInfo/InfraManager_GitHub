using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.Settings
{
    public class EmailQuoteTrimmer
    {
        public Guid ID { get; init; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public string From { get; set; }
        public byte FromType { get; set; }
        public string Theme { get; set; }
        public byte ThemeType { get; set; }
        public byte[] RowVersion { get; set; }
        public string Body { get; set; }
        public byte BodyType { get; set; }
    }
}
