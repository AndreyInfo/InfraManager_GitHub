using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace InfraManager.Core.Readers
{
    [Serializable()]
    public sealed class CSVReaderException : ApplicationException
    {
        private const string __csvExceptionMessage = "The CSV appears to be corrupt near record '{0}' field '{1} at position '{2}'. Current raw data : '{3}'.";


        private string _message;
        private string _rawData;
        private int _currentFieldIndex;
        private long _currentRecordIndex;
        private int _currentPosition;



        public CSVReaderException()
            : this(null, null)
        { }

        public CSVReaderException(string message)
            : this(message, null)
        { }

        public CSVReaderException(string message, Exception innerException)
            : base(string.Empty, innerException)
        {
            _message = (message == null ? string.Empty : message);
            _rawData = string.Empty;
            _currentPosition = -1;
            _currentRecordIndex = -1;
            _currentFieldIndex = -1;
        }

        public CSVReaderException(string rawData, int currentPosition, long currentRecordIndex, int currentFieldIndex)
            : this(rawData, currentPosition, currentRecordIndex, currentFieldIndex, null)
        { }

        public CSVReaderException(string rawData, int currentPosition, long currentRecordIndex, int currentFieldIndex, Exception innerException)
            : base(String.Empty, innerException)
        {
            _rawData = (rawData == null ? string.Empty : rawData);
            _currentPosition = currentPosition;
            _currentRecordIndex = currentRecordIndex;
            _currentFieldIndex = currentFieldIndex;
            _message = string.Format(CultureInfo.InvariantCulture, __csvExceptionMessage, _currentRecordIndex, _currentFieldIndex, _currentPosition, _rawData);
        }

        private CSVReaderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _message = info.GetString("CSVMessage");
            _rawData = info.GetString("RawData");
            _currentPosition = info.GetInt32("CurrentPosition");
            _currentRecordIndex = info.GetInt64("CurrentRecordIndex");
            _currentFieldIndex = info.GetInt32("CurrentFieldIndex");
        }

        public sealed override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("CSVMessage", _message);
            info.AddValue("RawData", _rawData);
            info.AddValue("CurrentPosition", _currentPosition);
            info.AddValue("CurrentRecordIndex", _currentRecordIndex);
            info.AddValue("CurrentFieldIndex", _currentFieldIndex);
        }



        #region Properties
        public sealed override string Message
        {
            get { return _message; }
        }

        public string RawData
        {
            get { return _rawData; }
        }

        public int CurrentPosition
        {
            get { return _currentPosition; }
        }

        public long CurrentRecordIndex
        {
            get { return _currentRecordIndex; }
        }

        public int CurrentFieldIndex
        {
            get { return _currentFieldIndex; }
        }
        #endregion
    }
}
