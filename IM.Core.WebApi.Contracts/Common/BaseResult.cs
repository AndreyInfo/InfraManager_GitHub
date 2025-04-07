using System;

namespace InfraManager.CrossPlatform.WebApi.Contracts.Common
{
    /// <summary>
    /// Базовый класс для ответов
    /// </summary>
    /// <typeparam name="T">тип результата ответа</typeparam>
    /// <typeparam name="F">тип отрицательного ответа</typeparam>
    [Obsolete("Try to not use it anymore")]
    public class BaseResult<T, F>
        where F : struct, Enum
    {
        /// <summary>
        /// Конструктор 
        /// </summary>        
        public BaseResult(T result, F? fault = default)
        {
            Result = result;
            Success = fault == null; 
            Fault = fault;
        }

        /// <summary>
        /// Результат выполнения операции
        /// </summary>
        public T Result { get; private set; }

        /// <summary>
        /// Положительный или отрицательный результат
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Отрицательный результат выполнения операции
        /// </summary>
        public F? Fault { get; private set; }
    }
}
