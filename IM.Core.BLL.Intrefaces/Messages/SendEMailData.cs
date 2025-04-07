using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages
{
    public class SendEMailData
    {
        public string ToAddresses { get; init; }
        public string BccAddresses { get; init; }
        public Guid ObjectID { get; init; }
        public string HtmlBody { get; init; }
        public string Subject { get; init; }
        public List<FileInfo> Files { get; init; }

        public class FileInfo
        {
            public Guid? ID { get; init; }

            public Guid? ObjectID { get; init; }

            public string FileName { get; init; }

        }
    }
}
