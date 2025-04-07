using InfraManager.Core;
using InfraManager.Core.Logging;
using InfraManager.ServiceBase.MailService.WebAPIModels;
using InfraManager.Services;
using InfraManager.Services.MailService;
using System;
using System.Text;

namespace InfraManager.BLL.Settings.MailService
{
    public static class MailManager
    {
        #region fields
        private const string __serviceName = "mailService";
        private static readonly object[] __arguments = { };
        private static readonly object __lock = new object();
        private static bool __isConnected = false;
        private static DateTime? __lastUtcDateConnecting = null;

        private static IMailService _mailService;
        private static object _locker = new object();
        #endregion


        #region properties
        public static bool IsConnected { get { return __isConnected; } }
        #endregion


        #region events
        public static event EventHandler Connected;

        public static event EventHandler Disconnected;
        #endregion


        #region constructors
        static MailManager()
        {
            if (!ServiceManager.Instance.IsInitialized)
                lock (ServiceManager.StaticLock)
                    if (!ServiceManager.Instance.IsInitialized)
                    {
                        ServiceManager.Instance.Initialize();
                        ServiceManager.Instance.Start();
                    }
        }
        #endregion


        #region method Connect
        public static void Connect()
        {
            if (!__isConnected)
                try
                {
                    if (__lastUtcDateConnecting.HasValue && DateTime.UtcNow.Subtract(__lastUtcDateConnecting.Value).TotalSeconds < 10)
                        return;
                    __lastUtcDateConnecting = DateTime.UtcNow;
                    var service = GetService();
                    if (service != null)
                    {
                        try
                        {
                            __isConnected = service.EnsureAvailability().Type == OperationResultType.Success;
                        }
                        catch
                        {
                            __isConnected = false;
                        }
                        if (__isConnected)
                            OnConnected();
                    }
                }
                catch (Exception e)
                {
                    Logger.Debug(e, "Ошибка при подключении к почтовому сервису.");
                }
        }
        #endregion

        #region method Disconnect
        public static void Disconnect()
        {
            __lastUtcDateConnecting = null;
            if (__isConnected)
                try
                {
                    ServiceManager.Instance.ReleaseService(__serviceName);
                }
                catch (Exception e)
                {
                    Logger.Debug(e, "Ошибка при отключении от почтового сервиса.");
                }
                finally
                {
                    __isConnected = false;
                    OnDisconnected();
                }
        }
        #endregion

        #region method TestConnection
        public static void TestConnection()
        {
            if (__isConnected)
                try
                {
                    var service = GetService();
                    if (service == null || service.EnsureAvailability().Type == OperationResultType.Failure)
                        Disconnect();
                }
                catch (Exception e)
                {
                    Disconnect();
                    Logger.Debug(e, "Ошибка при проверке доступности почтового сервиса.");
                }
        }

        public static void TestConnection(string scheme, string host, int port)
        {
            ServiceManager.Instance.TestConnection<IMailService>(__serviceName, scheme, host, port, __arguments);
        }
        #endregion

        #region static method SendMail
        public static void SendMail(SMTPMessage message)
        {
            if (!__isConnected)
                Connect();
            //
            string msgInfo = null;
            Exception exception = null;
            if (__isConnected)
            {
                try
                {
                    var service = GetService();
                    if (service != null)
                    {
                        var operationResult = service.SendMail(message);
                        if (operationResult.Type == OperationResultType.Failure)
                            msgInfo = "Неудача при отправке email (на стороне почтовой службы IM).";
                    }
                    else
                        msgInfo = "Не найдена служба MailService.";


                }
                catch (Exception ex)
                {
                    msgInfo = "Ошибка при отправке email.";
                    exception = ex;
                }
            }
            else
                msgInfo = "Нет соединения с MailService.";
            //
            if (!string.IsNullOrWhiteSpace(msgInfo))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Сообщение email не будет отправлено. Причина: " + msgInfo);
                sb.Append("\r\n\tTo: ");
                foreach (var tmp in message.To)
                    sb.Append(tmp.Address + "; ");
                sb.Append("\r\n\tSubject: " + message.Subject);
                sb.Append("\r\n\tAttachmentCount: " + message.Attachments.Count);
                //
                if (exception == null)
                    Logger.Verbose(sb.ToString());
                else
                    Logger.Error(exception, sb.ToString());
            }
        }
        #endregion

        #region static method GetSMTPServer
        public static string GetSMTPServer()
        {
            if (!__isConnected)
                Connect();
            //
            string retval = null;
            if (__isConnected)
                try
                {
                    var service = GetService();
                    if (service != null)
                    {
                        var operationResult = service.GetSMTPServer(out retval);
                        if (operationResult.Type == OperationResultType.Failure)
                        {
                            //
                            //TODO:
                            //                                
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка при получении адреса сервера исходящей почты.");
                }
            return retval;
        }
        #endregion

        #region static method GetSMTPPort
        public static int GetSMTPPort()
        {
            if (!__isConnected)
                Connect();
            //
            int retval = -1;
            if (__isConnected)
                try
                {
                    var service = GetService();
                    if (service != null)
                    {
                        var operationResult = service.GetSMTPPort(out retval);
                        if (operationResult.Type == OperationResultType.Failure)
                        {
                            //
                            //TODO:
                            //                                
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка при получении порта сервера исходящей почты.");
                }
            return retval;
        }
        #endregion

        #region static method GetMaxAttachmentSize
        public static int? GetMaxAttachmentSize()
        {
            if (!__isConnected)
                Connect();
            //
            int? retval = null;
            if (__isConnected)
                try
                {
                    var service = GetService();
                    if (service != null)
                    {
                        var operationResult = service.GetMaxAttachmentSize(out retval);
                        if (operationResult.Type == OperationResultType.Failure)
                        {
                            //
                            //TODO:
                            //                                
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка при получении максимального размера прикрепления.");
                }
            return retval;
        }
        #endregion



        #region private method OnConnected
        private static void OnConnected()
        {
            var connected = Connected;
            if (connected != null)
                connected(null, System.EventArgs.Empty);
        }
        #endregion

        #region private method OnDisconnected
        private static void OnDisconnected()
        {
            var disconnected = Disconnected;
            if (disconnected != null)
                disconnected(null, System.EventArgs.Empty);
        }
        #endregion

        #region Settings
       

        public static OperationResult TestSMTPSettings(SMTPSettingsTest testData)
        {
            var operationResult = new OperationResult() {Type = OperationResultType.Failure };
            if (!__isConnected)
                Connect();

            if (__isConnected)
                try
                {
                    var service = GetService();
                    if (service != null)
                    {
                       operationResult = service.TestSMTPSettings(testData);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка при подключении к сервису почты.");
                }
                finally
                {
                    Disconnect();
                }
            return operationResult;
        }

        public static OperationResult TestInboundProtocolSettings(POPSettingsTest testData)
        {
            var operationResult = new OperationResult() { Type = OperationResultType.Failure };
            if (!__isConnected)
                Connect();

            if (__isConnected)
                try
                {
                    var service = GetService();
                    if (service != null)
                    {
                        operationResult = service.TestInboundProtocolSettings(testData);
                        if (operationResult.Type == OperationResultType.Failure)
                        {
                            Logger.Error("Ошибка при подключении к сервису почты.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка при подключении к сервису почты.");
                }
                finally
                {
                    Disconnect();
                }

            return operationResult;
        }

        #endregion
        private static IMailService GetService()
        {
            lock (_locker)
            {
                if (_mailService == null)
                    _mailService = new WebAPIClient.MailServiceClient(ApplicationManager.Instance.MailServiceBaseURL);
                return _mailService;
            }
        }

    }
}