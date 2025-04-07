namespace InfraManager.DAL.Sessions;

public enum SessionLicenceType : short
{
    /// <summary>
    /// Именная лицензия
    /// </summary>
    Personal = 0,
    
    /// <summary>
    /// Конкурентная лицензия
    /// </summary>
    Concurrency = 1
}