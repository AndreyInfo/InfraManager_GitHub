using InfraManager.BLL.ServiceDesk;
using InfraManager.Services.MailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Messages
{
    internal class SendEMailBLL : ISendEMailBLL, ISelfRegisteredService<ISendEMailBLL>
    {
        private readonly IMailService _mailService;
        private readonly IDocumentBLL _documentBLL;

        public SendEMailBLL(
            IMailService mailService,
            IDocumentBLL documentBLL)
        {
            _mailService = mailService;
            _documentBLL = documentBLL;
        }

        public async Task<bool> SendEMailAsync(SendEMailData data, CancellationToken cancellationToken)
        {
            SMTPMessage sendData = new SMTPMessage();
            sendData.To.AddRange(ToAddressList(data.ToAddresses));
            sendData.BCC.AddRange(ToAddressList(data.BccAddresses));
            sendData.Subject = data.Subject;
            sendData.Body = data.HtmlBody;
            sendData.IsBodyHTML = true;
            if(data.Files?.Any() ?? false)
            {
                foreach(var file in data.Files)
                {
                    sendData.Attachments.Add(await PrepairAttachmentAsync(file, cancellationToken));
                }
            }

            var result = _mailService.SendMail(sendData);
            return result.Type == Services.OperationResultType.Success;
        }

        private async Task<SMTPAttachment> PrepairAttachmentAsync(SendEMailData.FileInfo file, CancellationToken cancellationToken)
        {
            var fileData = await _documentBLL.GetDocumentDataAsync(file.ID.Value, cancellationToken);
            return new SMTPAttachment(file.FileName, fileData.Data);
        }

        private IEnumerable<SMTPMailAddress> ToAddressList(string toAddresses)
        {
            if (toAddresses == null)
                return Enumerable.Empty<SMTPMailAddress>();
            var list = toAddresses.Split(new char[] { ',',';' }, StringSplitOptions.RemoveEmptyEntries);
            return list.Select(x => new SMTPMailAddress(x));
        }
    }
}
