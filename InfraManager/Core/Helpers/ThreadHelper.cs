using System;
using System.Collections.Generic;

namespace InfraManager.Core.Helpers
{
    /// <summary>
    /// Вспомогательный класс для работы с потоками
    /// </summary>
    public sealed class ThreadHelper
    {
        #region Fields
        /// <summary>
        /// Словарь именованных значений, который позволяет хранить данные, специфичные для текущего потока
        /// </summary>
        [ThreadStatic]
        private static Dictionary<string, object> __slots;
        #endregion

        #region static method GetData
        /// <summary>
        /// Возвращает потокоспецифичную информацию, находящуюся в слоте с указанным наименованием
        /// </summary>
        /// <typeparam name="T">Тип возвращаемых данных</typeparam>
        /// <param name="slot">Наименование слота</param>
        /// <returns>Потокоспецифичная информация, находящаяся в слоте с указанным наименованием</returns>
        /// <exception cref="T:System.ArgumentNullException"/>
        /// <exception cref="T:System.ArgumentException"/>
        public static T GetData<T>(string slot)
        {
            if (slot == null)
                throw new ArgumentNullException("slot");

            CheckSlots();

            if (!__slots.ContainsKey(slot))
                return default(T);
            else
                return (T)__slots[slot];
        }
        #endregion

        #region static method SetData
        /// <summary>
        /// Сохраняет потокоспецифичную информацию в слоте
        /// </summary>
        /// <param name="slot">Наименование слота</param>
        /// <param name="data">Данные слота</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public static void SetData(string slot, object data)
        {
            if (slot == null)
                throw new ArgumentNullException("slot");

            CheckSlots();

            if (__slots.ContainsKey(slot))
                __slots[slot] = data;
            else
                __slots.Add(slot, data);
        }
        #endregion

        #region static method RemoveData
        /// <summary>
        /// Удаляет потокоспецифичную информацию с указанным наименованием слота
        /// </summary>
        /// <param name="slot">Наименование слота</param>
        /// <exception cref="T:System.ArgumentNullException"/>
        public static void RemoveData(string slot)
        {
            if (slot == null)
                throw new ArgumentNullException("slot");

            CheckSlots();

            __slots.Remove(slot);
        }
        #endregion

        #region private static method CheckSlots
        /// <summary>
        /// Производит проверку инициилизирован ли внутренний словарь и при необходимости его инициализирует
        /// </summary>
        private static void CheckSlots()
        {
            if (__slots == null)
                __slots = new Dictionary<string, object>();
        }
        #endregion
    }
}
