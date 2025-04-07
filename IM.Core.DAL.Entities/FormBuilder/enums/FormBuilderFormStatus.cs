namespace InfraManager.DAL.FormBuilder.Enums;

public enum FormBuilderFormStatus
{
    /// <summary>
    /// Описывает состояние формы, когда она только создана
    /// </summary>
    Created = 0,
    
    /// <summary>
    /// Описывает состояние формы, когда она была опубликована
    /// </summary>
    Published = 1,
    
    /// <summary>
    /// Описывает состояние формы, когда она была переопределена
    /// </summary>
    Overriden = 2,
    
    /// <summary>
    /// Описывает состояние формы, когда она была заблокирована
    /// </summary>
    Blocked = 3
}
