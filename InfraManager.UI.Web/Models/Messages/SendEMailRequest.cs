using System.Collections.Generic;
using System;

namespace InfraManager.UI.Web.Models.Messages
{
    public class SendEMailRequest
    {
        public string ToAddresses { get; set; }
        public string BccAddresses { get; set; }
        public Guid KBArticleID { get; set; }
        public string HtmlBody { get; set; }
        public string Subject { get; set; }
        public List<InfraManager.Web.DTL.Repository.UploadFileInfo> Files { get; set; }

    }
}
