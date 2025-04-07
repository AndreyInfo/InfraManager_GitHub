using InfraManager.DAL.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Accounts
{
    public class UserAccountData
    {
        public UserAccountTypes Type { get; init; }
        
        /// <summary>
        /// Название - Строка длиной не более 50 символов 
        /// </summary>
        public string Name { get; init; }
        
        /// <summary>
        /// Логин - Строка длиной не более 50 символов 
        /// </summary>
        public string Login { get; set; }
        
        /// <summary>
        /// Пароль - Строка длиной не более 50 символов 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Теги - перечень слов(последовательность символов, не включающих проблемы и знаки препинания), разделенных запятыми
        /// </summary>
        //public virtual IList<Tag> Tags { get; set; } // TODO Will need for implemente the Tags

        /// <summary>
        /// SSH passphrase - Строка длиной не более 50 символов(применимо только для типа SSH)
        /// </summary>
        public string SSH_Passphrase { get; set; }
        
        /// <summary>
        /// SSH Private key - Строка длиной не более 50 символов(применимо только для типа SSH) 
        /// </summary>
        public string SSH_PrivateKey { get; set; }
        
        /// <summary>
        /// Authentication protocol - Выбор из перечнья:MD5, SHA-1, SHA-224, SHA-256, SHA-384, SHA-512(применимо только для типа SNMP v3) 
        /// </summary>
        public HashAlgorithms AuthenticationProtocol { get; set; }
        
        /// <summary>
        /// Authentication Key - Строка длиной не более 50 символов(применимо только для типа SNMP v3)
        /// </summary>
        public string AuthenticationKey { get; set; }
        
        /// <summary>
        /// Privacy protocol - Выбор из перечнья:3DES, AES128, AES192, AES256,DES(применимо только для типа SNMP v3) 
        /// </summary>
        public CryptographicAlgorithms PrivacyProtocol { get; set; }
        
        /// <summary>
        /// Privacy key - Строка длиной не более 50 символов(применимо только для типа SNMP v3)
        /// </summary>
        public string PrivacyKey { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
