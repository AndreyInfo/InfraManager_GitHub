namespace InfraManager.DAL.ServiceDesk
{
    public enum CallReceiptType : byte
    {
        //[FriendlyName("Телефон"), FriendlyName("Phone", "en-US")]
        Phone = 0,

        //[FriendlyName("E-mail")]
        Email = 1,

        //[FriendlyName("Web-интерфейс"), FriendlyName("Web portal", "en-US")]
        Web = 2,

        //[FriendlyName("Почта"), FriendlyName("Mail", "en-US")]
        Mail = 3,

        //[FriendlyName("Система"), FriendlyName("System", "en-US")]
        System = 4,

        //[FriendlyName("Процесс"), FriendlyName("Process", "en-US")]
        Process = 5,//не трогать - нарушение какого-либо процесса

        //[FriendlyName("Служебная записка"), FriendlyName("Official note", "en-US")]
        ServiceNote = 6
    }
}
