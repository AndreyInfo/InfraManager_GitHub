namespace InfraManager
{
    public enum ConditionEnum : byte
    {
        /// <summary>
        /// Равно
        /// </summary>
        Equals = 0,

        /// <summary>
        /// Не равно
        /// </summary>
        NotEquals = 1,

        /// <summary>
        /// Содержит
        /// </summary>
        Contains = 2,

        /// <summary>
        /// Не содержит
        /// </summary>
        NotContains = 3,

        /// <summary>
        /// Больше, чем
        /// </summary>
        MoreThan = 4,

        /// <summary>
        /// Меньше, чем
        /// </summary>
        LessThan = 5,

        /// <summary>
        /// Между
        /// </summary>
        Between = 6
    }
}
