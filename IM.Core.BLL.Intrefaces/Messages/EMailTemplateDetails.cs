namespace InfraManager.BLL.Messages;

public class EMailTemplateDetails
{
    /// <summary>
    /// Тема письма
    /// </summary>
    public string Subject { get; init; }
    
    /// <summary>
    /// Тело письма
    /// </summary>
    public string Body { get; init; }
}