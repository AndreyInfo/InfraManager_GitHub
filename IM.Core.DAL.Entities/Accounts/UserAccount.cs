using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Accounts
{
    [ObjectClassMapping(ObjectClass.UserAccount)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.UserAccount_Delete)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.UserAccount_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.UserAccount_Update)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.UserAccount_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.UserAccount_Properties)]
    public class UserAccount : IMarkableForDelete
    {
        protected UserAccount()
        {
        }

        public UserAccount(string name)
        {
            Name = name;
            CreateDate = DateTime.UtcNow;
        }

        public int ID { get; init; }

        /// <summary>
        /// Тип - выбор из перечня: Общего назначения, Приложение, Windows, VMware, SSH, CIM, SNMP v2, SNMP v3 
        /// </summary>
        public UserAccountTypes Type { get; set; }

        /// <summary>
        /// Название - Строка длиной не более 50 символов 
        /// </summary>
        public string Name { get; set; }
        
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
        /// Authentication protocol - Выбор из перечнья:MD5, SHA-1, SHA-224, SHA-256, SHA-384, SHA-512(применимо только для типа SNMP v3)
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

        public bool Removed { get; private set; }

        public void MarkForDelete()
        {
            Removed = true; 
            RemovedDate = DateTime.UtcNow;
        }

        public DateTime CreateDate { get; set; }
        
        public DateTime? RemovedDate { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
