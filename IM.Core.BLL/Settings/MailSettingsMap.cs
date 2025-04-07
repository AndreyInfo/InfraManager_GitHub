using AutoMapper;
using InfraManager.BLL.ServiceDesk.DTOs;
using InfraManager.DAL.ServiceDesk;
using InfraManager.ServiceBase.MailService.WebAPIModels;

namespace InfraManager.BLL.Settings
{
    public class MailSettings : Profile
    {
        public MailSettings()
        {
            CreateMap<MailServiceSettings, ConnectionSettings>()
                 .ForPath(dst => dst.PostService.MailServicePort, m => m.MapFrom(scr => scr.Port))
                 .ForPath(dst => dst.PostService.EnableTracing, m => m.MapFrom(scr => scr.EnableTrace))
                 .ForPath(dst => dst.ConnectingWorkflowService.ServerNameOrIPAddress, m => m.MapFrom(scr => scr.WorkflowServiceBaseURL.Substring(0, scr.WorkflowServiceBaseURL.LastIndexOf(':'))))
                 .ForPath(dst => dst.ConnectingWorkflowService.WorkProcedureServicePort, m => m.MapFrom(scr => scr.WorkflowServiceBaseURL.Substring(scr.WorkflowServiceBaseURL.LastIndexOf(':') + 1).Replace("/", "")));
            CreateMap<ConnectionSettings, MailServiceSettings>()
                 .ForMember(dst => dst.Port, m => m.MapFrom(scr => scr.PostService.MailServicePort))
                 .ForMember(dst => dst.EnableTrace, m => m.MapFrom(scr => scr.PostService.EnableTracing))
                 .ForPath(dst => dst.WorkflowServiceBaseURL, m => m.MapFrom(scr => scr.ConnectingWorkflowService.ServerNameOrIPAddress + ":" + scr.ConnectingWorkflowService.WorkProcedureServicePort))
                 .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<MailServiceSettings, SendMailSettings>()
               .ForPath(dst => dst.Authentication.Login, m => m.MapFrom(scr => scr.MailManagerConfiguration.SMTPUserName))
               .ForPath(dst => dst.Authentication.UseAuthentication, m => m.MapFrom(scr => scr.MailManagerConfiguration.SMTPAutenticationRequired))
               .ForPath(dst => dst.Authentication.Password, m => m.MapFrom(scr => Core.Helpers.SecurityHelper.Decrypt(scr.MailManagerConfiguration.SMTPUserPasswordEncrypted)))
               .ForPath(dst => dst.SendEmail.OutgoingServerPort, m => m.MapFrom(scr => scr.MailManagerConfiguration.SMTPPort))
               .ForPath(dst => dst.SendEmail.SenderEmail, m => m.MapFrom(scr => scr.MailManagerConfiguration.SMTPSenderEmail))
               .ForPath(dst => dst.SendEmail.SenderName, m => m.MapFrom(scr => scr.MailManagerConfiguration.SMTPSenderName))
               .ForPath(dst => dst.SendEmail.SMTP, m => m.MapFrom(scr => scr.MailManagerConfiguration.SMTPServer))
               .ForPath(dst => dst.SendEmail.Timeout, m => m.MapFrom(scr => scr.MailManagerConfiguration.SMTPTimeout))
               .ForPath(dst => dst.SendEmail.UseSLL, m => m.MapFrom(scr => scr.MailManagerConfiguration.IsSSLEnabled))
               .ForPath(dst => dst.SendEmail.IgnoreSertificates,
                   m => m.MapFrom(scr => scr.MailManagerConfiguration.IgnoreSertificates))
               .ForAllOtherMembers(opts => opts.Ignore());


            CreateMap<SendMailSettings, MailServiceSettings>()
                .ForPath(dst => dst.MailManagerConfiguration.SMTPUserName,
                    m => m.MapFrom(scr => scr.Authentication.Login))
                .ForPath(dst => dst.MailManagerConfiguration.SMTPAutenticationRequired,
                    m => m.MapFrom(scr => scr.Authentication.UseAuthentication))
                .ForPath(dst => dst.MailManagerConfiguration.SMTPUserPasswordEncrypted,
                    m => m.MapFrom(scr => Core.Helpers.SecurityHelper.Encrypt(scr.Authentication.Password)))
                .ForPath(dst => dst.MailManagerConfiguration.SMTPPort,
                    m => m.MapFrom(scr => scr.SendEmail.OutgoingServerPort))
                .ForPath(dst => dst.MailManagerConfiguration.SMTPSenderEmail,
                    m => m.MapFrom(scr => scr.SendEmail.SenderEmail))
                .ForPath(dst => dst.MailManagerConfiguration.SMTPSenderName,
                    m => m.MapFrom(scr => scr.SendEmail.SenderName))
                .ForPath(dst => dst.MailManagerConfiguration.SMTPServer, m => m.MapFrom(scr => scr.SendEmail.SMTP))
                .ForPath(dst => dst.MailManagerConfiguration.SMTPTimeout, m => m.MapFrom(scr => scr.SendEmail.Timeout))
                .ForPath(dst => dst.MailManagerConfiguration.IsSSLEnabled, m => m.MapFrom(scr => scr.SendEmail.UseSLL))
                .ForPath(x => x.MailManagerConfiguration.IgnoreSertificates,
                    x => x.MapFrom(x => x.SendEmail.IgnoreSertificates));

            CreateMap<MailServiceSettings, GetMailSettings>()
                .ForPath(dst => dst.CheckingMail.CheckInbox,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.CheckForMail))
                .ForPath(dst => dst.CheckingMail.CheckInterval,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.CheckInterval))
                .ForPath(dst => dst.CheckingMail.HandleAttachments,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.ProcessAttachments))
                .ForPath(dst => dst.CheckingMail.HandleMessagesNotBody,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.IsToProcessEmailWithoutContent))
                .ForPath(dst => dst.CheckingMail.HandleMessagesNotSubject,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.IsToProcessEmailWithoutTitle))
                .ForPath(dst => dst.CheckingMail.RespondClient,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.ReplyToClient))
                .ForPath(dst => dst.CheckingMail.MaximumAttachmentSize,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.MaxAttachmentSize))
                .ForPath(dst => dst.ReceivingMail.CurrentProtocol,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.InboundProtocol))
                .ForPath(dst => dst.ReceivingMail.Login,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.POP3UserName))
                .ForPath(dst => dst.ReceivingMail.MailServer,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.POP3Server))
                .ForPath(dst => dst.ReceivingMail.MailServerPort,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.POP3Port))
                .ForPath(dst => dst.ReceivingMail.UseSLL,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.InboundUseSsl))
                .ForPath(dst => dst.ReceivingMail.Password,
                    m => m.MapFrom(scr =>
                        Core.Helpers.SecurityHelper.Decrypt(scr.MailManagerConfiguration.POP3UserPasswordEncrypted)))
                .ForPath(dst => dst.CheckingMail.CheckInbox,
                    m => m.MapFrom(scr => scr.MailManagerConfiguration.CheckForMail));

            CreateMap<GetMailSettings, MailServiceSettings>()
               .ForPath(dst => dst.MailManagerConfiguration.CheckForMail, m => m.MapFrom(scr => scr.CheckingMail.CheckInbox))
               .ForPath(dst => dst.MailManagerConfiguration.CheckInterval, m => m.MapFrom(scr => scr.CheckingMail.CheckInterval))
               .ForPath(dst => dst.MailManagerConfiguration.ProcessAttachments, m => m.MapFrom(scr => scr.CheckingMail.HandleAttachments))
               .ForPath(dst => dst.MailManagerConfiguration.IsToProcessEmailWithoutContent, m => m.MapFrom(scr => scr.CheckingMail.HandleMessagesNotBody))
               .ForPath(dst => dst.MailManagerConfiguration.IsToProcessEmailWithoutTitle, m => m.MapFrom(scr => scr.CheckingMail.HandleMessagesNotSubject))
               .ForPath(dst => dst.MailManagerConfiguration.ReplyToClient, m => m.MapFrom(scr => scr.CheckingMail.RespondClient))
               .ForPath(dst => dst.MailManagerConfiguration.MaxAttachmentSize, m => m.MapFrom(scr => scr.CheckingMail.MaximumAttachmentSize))
               .ForPath(dst => dst.MailManagerConfiguration.InboundProtocol, m => m.MapFrom(scr => scr.ReceivingMail.CurrentProtocol))
               .ForPath(dst => dst.MailManagerConfiguration.POP3UserName, m => m.MapFrom(scr => scr.ReceivingMail.Login))
               .ForPath(dst => dst.MailManagerConfiguration.POP3Server, m => m.MapFrom(scr => scr.ReceivingMail.MailServer))
               .ForPath(dst => dst.MailManagerConfiguration.POP3Port, m => m.MapFrom(scr => scr.ReceivingMail.MailServerPort))
               .ForPath(dst => dst.MailManagerConfiguration.InboundUseSsl, m => m.MapFrom(scr => scr.ReceivingMail.UseSLL))
               .ForPath(dst => dst.MailManagerConfiguration.POP3UserPasswordEncrypted, m => m.MapFrom(scr => Core.Helpers.SecurityHelper.Encrypt(scr.ReceivingMail.Password)))
               ;

            CreateMap<MailServiceSettings, PolicySettings>()
             .ForMember(dst => dst.UseExceptions, m => m.MapFrom(scr => scr.MailManagerConfiguration.UseExceptions))
             .ForMember(dst => dst.Policy, m => m.MapFrom(scr => scr.MailManagerConfiguration.Policy ?? Core.PolicyType.Deny))
             .ForMember(dst => dst.ExceptionElements, m => m.MapFrom(scr => scr.MailManagerConfiguration.ExceptionElements));

            CreateMap<PolicySettings, MailServiceSettings>()
              .ForPath(dst => dst.MailManagerConfiguration.UseExceptions, m => m.MapFrom(scr => scr.UseExceptions))
              .ForPath(dst => dst.MailManagerConfiguration.Policy, m => m.MapFrom(scr => scr.Policy))
              .ForPath(dst => dst.MailManagerConfiguration.ExceptionElements, m => m.MapFrom(scr => scr.ExceptionElements))
              ;
        }
    }
}
