using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using InfraManager.DAL.DbConfiguration;
using InfraManager.DAL.Settings;

namespace InfraManager.DAL
{
    public abstract class ConnectionStringProviderWithEncryption : IConnectionStringProvider
    {
        private static string SecurityKey = "jd89$32#90JHgwjn%MLwhb3b";
        protected readonly string user, password, connectionString;
        private readonly IAppSettingsEditor _appSettingsEditor;
        private readonly IOldDataSourceLocatorEditor _oldDataSourceLocatorEditor;

        public ConnectionStringProviderWithEncryption(IConfiguration configuration,
            IAppSettingsEditor appSettingsEditor,
            IOldDataSourceLocatorEditor oldDataSourceLocatorEditor)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _appSettingsEditor = appSettingsEditor;
            _oldDataSourceLocatorEditor = oldDataSourceLocatorEditor;

            user = configuration["dbUser"];
            password = configuration["dbPassword"];
            connectionString = configuration.GetConnectionString(ConnectionStringSettingName);
        }

        protected void BaseChangeConnectionString(string connectionString, string user = null, string password = null)
        {
            List<(string, string)> parametersToUpdate = new List<(string, string)>();

            if (!string.IsNullOrEmpty(user))
                parametersToUpdate.Add(("dbUser", Encrypt(user)));
            if (!string.IsNullOrEmpty(password))
                parametersToUpdate.Add(("dbPassword", Encrypt(password)));

            parametersToUpdate.Add(($"ConnectionStrings:{ConnectionStringSettingName}", connectionString));

            _appSettingsEditor.Edit(parametersToUpdate);
        }

        protected void BaseChangeOldDataSource(string connectionString)
        {
            _oldDataSourceLocatorEditor.Edit(connectionString);
        }

        public abstract string GetConnectionString();
        public abstract (string Server, string Database, string Login, string Password, int Port, int CommandTimeout) GetConnectionObject();

        public abstract void ChangeConnectionString(string connectionString);

        public abstract string BuildConnectionString(string server, string database, string login, string password,
            string additionalField, int port);

        protected string Decrypt(string CipherText)
        {
            byte[] toEncryptArray = Convert.FromBase64String(CipherText);
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(Encoding.UTF8.GetBytes(SecurityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;
            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCryptoTransform = objTripleDESCryptoService.CreateDecryptor();
            //Transform the bytes array to resultArray
            byte[] resultArray = objCryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();

            //Convert and return the decrypted data/byte into string format.
            return Encoding.UTF8.GetString(resultArray);
        }

        protected string Encrypt(string PlainText)
        {
            // Getting the bytes of Input String.
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            //De-allocatinng the memory after doing the Job.
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;
            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;


            var objCryptoTransform = objTripleDESCryptoService.CreateEncryptor();
            //Transform the bytes array to resultArray
            byte[] resultArray = objCryptoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        protected abstract string ConnectionStringSettingName { get; }
    }
}
