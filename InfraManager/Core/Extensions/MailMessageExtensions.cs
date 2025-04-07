using System;
using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace InfraManager.Core.Extensions
{
    public static class MailMessageExtensions
    {
        #region Save
        //from reflector
        public static void Save(this MailMessage message, string fileName)
        {
            Assembly assembly = typeof(SmtpClient).Assembly;
            Type mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");
            //
            ConstructorInfo mailWriterContructor = mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(Stream) }, null);
            MethodInfo sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);
            //
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
            {
                object mailWriter = mailWriterContructor.Invoke(new object[] { fileStream });
                sendMethod.Invoke(message, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { mailWriter, true, true }, null);
                //
                MethodInfo closeMethod = mailWriter.GetType().GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);
                closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { }, null);
            }
        }
        #endregion
    }
}
