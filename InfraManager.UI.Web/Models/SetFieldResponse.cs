namespace InfraManager.Web.Models
{
    /// <summary>
    /// Универсальная модель, описывающая замену поля сущности.
    /// </summary>
    public sealed class SetFieldResponse
    {

        public ResultWithMessage ResultWithMessage { get; set; } = new ResultWithMessage();
    }

    /// <summary>
    /// Универсальная модлеь, описывающая результат изменеия сущности
    /// </summary>
    public sealed class ResultWithMessage
    {
        /// <summary>
        /// Результат
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
    }
}