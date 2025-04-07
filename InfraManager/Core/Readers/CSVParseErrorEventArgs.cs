using System;

namespace InfraManager.Core.Readers
{
    public sealed class CSVParseErrorEventArgs : System.EventArgs
    {
        #region Fields
        /// <summary>
        /// Ошибка
        /// </summary>
        private readonly CSVReaderException _exception;

        /// <summary>
        /// Действие по умолчанию
        /// </summary>
        private readonly ParseErrorAction _action;
        #endregion

        #region Properties
        /// <summary>
        /// Ошибка
        /// </summary>
        public CSVReaderException Exception
        {
            get
            {
                return _exception;
            }
        }

        /// <summary>
        /// Действие по умолчанию
        /// </summary>
        public ParseErrorAction Action
        {
            get
            {
                return _action;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="csvReaderException">Ошибка, о которой необходимо сообщить</param>
        /// <param name="defaultAction">Действие по умолчанию</param>
        /// <exception cref="System.ArgumentNullException"/>
        public CSVParseErrorEventArgs(CSVReaderException csvReaderException, ParseErrorAction defaultAction)
        {
            if (csvReaderException == null)
                throw new ArgumentNullException("csvReaderException");
            _exception = csvReaderException;
            _action = defaultAction;
        }
        #endregion
    }
}
