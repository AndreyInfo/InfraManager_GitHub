using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml;
using InfraManager.ComponentModel;
using InfraManager.Core.Extensions;
using InfraManager.Core.Helpers;

namespace InfraManager.Core
{
    public sealed class ApplicationManager : Singleton<ApplicationManager>
    {
        #region properties
        public string BaseDirectoryPath { get; private set; }

        public string ConfigurationFilePath { get; private set; }

        public string ProcessName { get; private set; }

        public DateTime ProcessStartTime { get; private set; }

        public string MachineName { get; private set; }

        public string OperatingSystem { get; private set; }

        public string[] IPAddresses { get; private set; }

        public string UserLogin { get; private set; }

        public bool IsWindowsApplication { get; set; }

        public bool IsWebApplication { get; set; }

        public Guid ApplicationID { get; private set; }

        public string ApplicationName { get; private set; }

        public string ApplicationVersion { get; set; }

        public bool IsInfraManager { get; private set; }

        public bool IsTelephonyService { get; private set; }

        public bool IsMailService { get; private set; }

        public bool IsWorkflowService { get; private set; }

        public int CurrentThreadID
        {
            get
            {
                try
                {
                    return Thread.CurrentThread.ManagedThreadId;
                }
                catch
                {
                    try
                    {
                        return Thread.CurrentThread.GetHashCode();
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
        }

        public string CurrentThreadName
        {
            get
            {
                try
                {
                    return Thread.CurrentThread.Name;
                }
                catch
                {
                    return this.CurrentThreadID.ToString();
                }
            }
        }

        public string CurrentThreadIdentity
        {
            get
            {
                try
                {
                    return Thread.CurrentPrincipal.Maybe(x => x.Identity).Maybe(x => x.Name) ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public string ApplicationFullName { get; private set; }
        public string WorkflowServiceBaseURL { get; set; }
        public string TelephonyServiceBaseURL { get; set; }
        public string SearchServiceBaseURL { get; set; }
        public string MailServiceBaseURL { get; set; }
        public string WebAPIBaseURL { get; set; }
        public string WebAPISecret { get; set; }
        public Guid WebAPIUserId { get; set; }
        public bool EnableTrace { get; set; }
        public string ImportServiceBaseURL { get; set; }
        public string ScheduledTasksPath { get; set; }
        public string DbType { get; set; }
        #endregion


        #region constructors
        static ApplicationManager() { }

        private ApplicationManager()
        {
            SetBaseDirectory();
            SetConfigurationFilePath();
            SetProcessName();
            SetProcessStartTime();
            SetMachineName();
            SetOperatingSystem();
            SetIPAddresses();
            SetUserLogin();
            SetApplicationSummary();
        }
        #endregion


        #region private method SetBaseDirectory
        private void SetBaseDirectory()
        {
            try
            {
                this.BaseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;
            }
            catch
            {
                this.BaseDirectoryPath = string.Empty;
            }
        }
        #endregion

        #region private method SetConfigurationFilePath
        private void SetConfigurationFilePath()
        {
            try
            {
                this.ConfigurationFilePath = ResourcesArea.Global.ExeConfigurationPath;
            }
            catch
            {
                this.ConfigurationFilePath = string.Empty;
            }
        }
        #endregion

        #region private method SetProcessName
        private void SetProcessName()
        {
            try
            {
                this.ProcessName = Process.GetCurrentProcess().ProcessName;
            }
            catch
            {
                this.ProcessName = string.Empty;
            }
        }
        #endregion

        #region private method SetProcessStartTime
        private void SetProcessStartTime()
        {
            try
            {
                this.ProcessStartTime = Process.GetCurrentProcess().StartTime;
            }
            catch
            {
                this.ProcessStartTime = DateTime.Now;
            }
        }
        #endregion

        #region private method SetMachineName
        private void SetMachineName()
        {
            try
            {
                this.MachineName = Dns.GetHostName();
            }
            catch
            {
                try
                {
                    this.MachineName = Environment.MachineName;
                }
                catch
                {
                    this.MachineName = string.Empty;
                }
            }
        }
        #endregion

        #region private method SetOperatingSystem
        private void SetOperatingSystem()
        {
            try
            {
                var operatingSystem = Environment.OSVersion;
                this.OperatingSystem = string.Concat(operatingSystem.VersionString, ", ", operatingSystem.Platform);
            }
            catch
            {
                this.OperatingSystem = string.Empty;
            }
        }
        #endregion

        #region private method SetIPAddresses
        private void SetIPAddresses()
        {
            try
            {
                this.IPAddresses = NetworkInterface.GetAllNetworkInterfaces().
                    Where(x => x.NetworkInterfaceType != NetworkInterfaceType.Loopback).
                    SelectMany(x => x.GetIPProperties().UnicastAddresses).
                    Select(x => x.Address.ToString()).
                    ToArray();
            }
            catch
            {
                try
                {
                    this.IPAddresses = Dns.GetHostAddresses(Environment.MachineName).Select(x => x.ToString()).ToArray();
                }
                catch
                {
                    this.IPAddresses = new string[0];
                }
            }
        }
        #endregion

        #region private method SetUserLogin
        private void SetUserLogin()
        {
            try
            {
                using (var windowsIdentity = WindowsIdentity.GetCurrent())
                    this.UserLogin = windowsIdentity.Name ?? string.Empty;
            }
            catch
            {
                this.UserLogin = string.Empty;
            }
        }
        #endregion

        #region private method SetApplicationSummary
        private void SetApplicationSummary()
        {
            this.ApplicationID = Guid.NewGuid();
            try
            {
                var xmlDocument = new XmlDocument().Do(x => x.Load(ResourcesArea.Global.ExeConfigurationPath));
                var loggingElement = xmlDocument.DocumentElement.SelectSingleNode(Application.ElementName);
                var stringBuilder = new StringBuilder();
                var xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings { Encoding = new UTF8Encoding(false) });
                xmlWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
                xmlWriter.WriteRaw(loggingElement.OuterXml);
                xmlWriter.Flush();
                var application = XmlHelper.XmlString2Object<Application>(stringBuilder.ToString());
                this.ApplicationName = application.Name;
                this.ApplicationVersion = application.Version;
                this.IsInfraManager = this.ApplicationName == "InfraManager";
                this.IsMailService = this.ApplicationName == "MailService";
                this.IsTelephonyService = this.ApplicationName == "TelephonyService";
                this.IsWorkflowService = this.ApplicationName == "WorkflowService";
            }
            catch
            {
                this.ApplicationName = string.Empty;
                this.ApplicationVersion = string.Empty;
            }
            //
            this.ApplicationFullName = string.Concat("ИнфраМенеджер ", this.ApplicationVersion);
        }
        #endregion

        #region public method SetApplicationFullName
        public void SetApplicationFullName(string fullName)
        {
            this.ApplicationFullName = fullName;
        }
        #endregion

        #region method PreloadAssemblies
        public void PreloadAssemblies()
        {
            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var dllDirPath = System.IO.Path.GetDirectoryName(path);
            //            
            //alex: не делать асинхронными - вешаются формы
            //while (!ThreadPool.QueueUserWorkItem((state) =>
            //{
            try
            {
                var a = System.Reflection.Assembly.LoadFile(System.IO.Path.Combine(dllDirPath, "InfraManager.CL.DE.dll"));
                var type = a.GetType("InfraManager.CL.DE.uctlRichTextEditor");
                Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                Logging.Logger.Error(ex);
            }
            //
            try
            {
                var a = System.Reflection.Assembly.LoadFile(System.IO.Path.Combine(dllDirPath, "InfraManager.CL.DE.dll"));
                var type = a.GetType("InfraManager.CL.DE.Dashboards.uctlDevExpressDashBoardViewer");
                var obj = Activator.CreateInstance(type);
                type.GetMethod("SetDashboardToNull").Invoke(obj, null);
            }
            catch (Exception ex)
            {
                Logging.Logger.Error(ex);
            }
            //}))
            //    Thread.Sleep(0);
            //
            //alex: тесты показали, что скорости не прибавляет, а время отнимает. К тому же, возникают проблемы с UI WPF
            //while (!ThreadPool.QueueUserWorkItem((state) =>
            //{
            //    var dlls = System.IO.Directory.GetFiles(dllDirPath, "InfraManager*.dll", System.IO.SearchOption.TopDirectoryOnly).
            //        Where(x =>
            //            {
            //                string fileName = System.IO.Path.GetFileName(x);
            //                return fileName != "InfraManager.Data.dll" && //envelope by HASP
            //                    fileName != "InfraManager.CL.Win.dll";//TODO: alex - problem with ShowDialog, STA thread fail!
            //            });
            //    foreach (var dll in dlls)
            //    {
            //        try
            //        {
            //            var a = System.Reflection.Assembly.LoadFile(dll);
            //            var types = a.GetTypes();
            //            foreach (var type in types)
            //            {
            //                var methods = type.GetMethods(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
            //                foreach (var method in methods)
            //                {
            //                    if ((method.Attributes & System.Reflection.MethodAttributes.Abstract) == System.Reflection.MethodAttributes.Abstract ||
            //                        method.ContainsGenericParameters)
            //                        continue;
            //                    //
            //                    System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle);
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Logging.Logger.Error(ex);
            //        }
            //   }
            //}))
            //    Thread.Sleep(0);
            #endregion
        }
    }
}