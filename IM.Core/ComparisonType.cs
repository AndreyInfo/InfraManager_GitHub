namespace InfraManager
{
    /// <summary>
    ///  Типы сравнения
    /// </summary>
    public enum ComparisonType
    {
        /// <summary>
        /// Не известный
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Вхождение строки
        /// </summary>
        Contains = 0,

        /// <summary>
        /// Равно
        /// </summary>
        Equal = 1,

        /// <summary>
        /// Не равно
        /// </summary>
        NotEqual = 2,

        /// <summary>
        /// Больше 
        /// </summary>
        GreaterThan = 3,

        /// <summary>
        /// Больше или равно
        /// </summary>
        GreaterThanOrEqual = 4,

        /// <summary>
        /// Меньше
        /// </summary>
        LessThan = 5,

        /// <summary>
        /// Меньше или равно
        /// </summary>
        LessThanOrEqual = 6,

        /// <summary>
        /// Начало строки
        /// </summary>
        StartsWith = 8,

        /// <summary>
        /// Окончание строки
        /// </summary>
        EndsWith = 9,
        
        /// <summary>
        /// Не вхождение строки
        /// </summary>
        NotContains = 10
    }
}
