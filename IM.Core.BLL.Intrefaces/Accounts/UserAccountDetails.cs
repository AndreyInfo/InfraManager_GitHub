namespace InfraManager.BLL.Accounts
{
    public class UserAccountDetails : UserAccountData
    {
        public int ID { get; init; }

        /// <summary>
        /// Тип - выбор из перечня: Общего назначения, Приложение, Windows, VMware, SSH, CIM, SNMP v2, SNMP v3 
        /// </summary>
        public string TypeText { get; init; }

        /// <summary>
        /// Authentication protocol - Выбор из перечнья:MD5, SHA-1, SHA-224, SHA-256, SHA-384, SHA-512(применимо только для типа SNMP v3) 
        /// </summary>
        public string AuthenticationProtocolText { get; set; }

        /// <summary>
        /// Privacy protocol - Выбор из перечнья:3DES, AES128, AES192, AES256,DES(применимо только для типа SNMP v3) 
        /// </summary>
        public string PrivacyProtocolText { get; set; }
    }
}
