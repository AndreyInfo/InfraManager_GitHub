namespace InfraManager.BLL.ExpressionBuilder
{
    /// <summary>
    /// Этот класс представляет информацию о положении части выражения в целом выражении
    /// </summary>
    public class ExpressionPart
    {
        /// <summary>
        /// Возвращает или задает значение номера индекса символа в выражении, с которого начинается часть выражения
        /// </summary>
        public int FromIndex { get; set; }

        /// <summary>
        /// Возвращает или задает значение индекса символа в выражении, на котором часть выражения заканчивается
        /// </summary>
        public int ToIndex { get; set; } 
    }
}
