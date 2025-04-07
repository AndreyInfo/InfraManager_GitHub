namespace InfraManager.BLL.FieldEdit
{
    /// <summary>
    /// результат сранврия занчния атрибутов объектов
    /// </summary>
    public enum FieldCompareResult
    {
        /// <summary>
        /// Значения атрибута совпадает
        /// </summary>
        Equal,
        /// <summary>
        /// Значение атрибута не совпадает
        /// </summary>
        NotEqual,
        /// <summary>
        /// Указано еимя атрибута не найдено в объектах
        /// </summary>
        InvalidField,
        /// <summary>
        /// Объекты разных типов
        /// </summary>
        ObjectTypeDiffers,
    }
}
