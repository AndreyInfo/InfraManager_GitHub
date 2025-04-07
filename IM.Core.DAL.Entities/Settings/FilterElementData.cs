using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Settings
{
    /// <summary>
    /// Элемент фильтра
    /// </summary>
    public sealed class FilterElementData
    {
        #region base

        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Id фильтра
        /// </summary>
        public Guid? FilterId { get; set; }

        /// <summary>
        /// Название свойства по которому фильтруем
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LocaleName { get; set; }

        /// <summary>
        /// Тип свойстава
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// Версия
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Пустое
        /// </summary>
        public byte IsEmpty { get; set; }

        /// <summary>
        /// Тип операции
        /// </summary>
        public byte? SearchOperation { get; set; }//круто было бы переименовать, так как теперь у всех операции, но хз как сконвертировать

        #endregion

        #region Datepick

        /// <summary>
        /// Дата начала
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public string FinishDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsStatic { get; set; }//obsolete

        /// <summary>
        /// 
        /// </summary>
        public byte? Interval { get; set; }//obsolete

        /// <summary>
        /// 
        /// </summary>
        public bool OnlyDate { get; set; }

        #endregion

        #region Range Slider

        /// <summary>
        /// 
        /// </summary>
        public string Left { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Right { get; set; }

        #endregion

        #region selector multiple

        /// <summary>
        /// 
        /// </summary>
        public IList<ValueData> SelectedValues { get; set; }

        #endregion

        #region selector value

        public string Selected { get; set; }

        #endregion


        #region  Simple value

        public IList<ValueData> Values { get; set; }//obsolete
        public IList<TreeValueData> TreeValues { get; set; }
        public string ClassSearcher { get; set; }
        public IList<string> SearcherParams { get; set; }
        public bool UseSelf { get; set; }

        #endregion

        #region Like Value

        public string SearchValue { get; set; }

        #endregion

        public bool? IsFilteredBySearcher { get; set; }       
    }
}
