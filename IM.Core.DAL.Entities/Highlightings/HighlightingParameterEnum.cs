namespace InfraManager.DAL.Highlightings
{
    public enum HighlightingParameterEnum : byte
    {
        /// <summary>
        /// Оставшееся время решения
        /// </summary>
        RemainingDecisionTime = 0,

        /// <summary>
        /// Прошло с момента поступления
        /// </summary>
        ElapsedSinceAdmission = 1,

        /// <summary>
        /// Оценка
        /// </summary>
        Grade = 2
    }
}
