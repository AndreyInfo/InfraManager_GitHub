using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using InfraManager.Core.Helpers;

namespace InfraManager.Core.Readers
{
    public sealed class CSVReader : ReaderBase, IEnumerable<string[]>
    {
        private const int __defaultBufferSize = 0x1000;
        private const char __defaultDelimiter = ',';
        private const char __defaultQuote = '"';
        private const char __defaultEscape = '"';
        private const char __defaultComment = '#';


        private const string __enumeratorStateIsInvalidMessage = "CSV Reader changed its position outside enumerator. Enumeration is impossible to start, continue or reset.";
        private const string __noHeadersMessage = "The CSV have no headers.";
        private const string __noCurrentRecordMessage = "No current record.";
        private const string __notEnoughSpaceInArrayMessage = "The number of fields in the record is greater than the available space from index to the end of the destination array.";
        private const string __supportedForwardOnlyModeMessage = "CSV reader supports a forward-only mode.";
        private const string __cannotReadRecordAtIndexMessage = "Cannot read record at index '{0}'.";
        private const string __parseErrorActionInvalidInsideParseErrorEventMessage = "'{0}' is not a valid ParseErrorAction while inside a ParseError event.";


        internal const string __fieldHeaderNotFoundMessage = "'{0}' field header not found.";

        internal const string __readerClosedMessage = "This operation is invalid when the reader is closed.";


        private static readonly StringComparer __headerComparer = StringComparer.CurrentCultureIgnoreCase;

        private TextReader _reader;

        private int _bufferSize;
        private char _delimiter;
        private char _quote;
        private char _escape;
        private char _comment;

        private bool _trimSpaces;
        private bool _hasHeaders;
        private ParseErrorAction _parseErrorAction = ParseErrorAction.ThrowException;
        private MissingFieldAction _missingFieldAction = MissingFieldAction.ThrowException;

        private bool _initialized;
        private string[] _headers;
        private Dictionary<string, int> _headerIndexes;
        private long _currentRecordIndex;

        private int _nextFieldStart;
        private int _nextFieldIndex;
        private string[] _fields;
        /// <summary>
        /// Contains the maximum number of fields to retrieve for each record.
        /// </summary>
        private int _fieldCount;
        private char[] _buffer;
        private int _bufferLength;

        private bool _eof;
        private bool _eol;
        /// <summary>
        /// Indicates if the first record is in cache.
        /// This can happen when initializing a reader with no headers
        /// because one record must be read to get the field count automatically
        /// </summary>
        private bool _firstRecordInCache;

        private bool _missingFieldFlag;
        private bool _parseErrorFlag;



        public CSVReader(string filePath)
            : this(new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), IOHelper.GetFileEncoding(filePath)), true)
        { }

        public CSVReader(string filePath, bool hasHeaders)
            : this(new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), IOHelper.GetFileEncoding(filePath)), hasHeaders)
        { }

        public CSVReader(TextReader reader, bool hasHeaders)
            : this(reader, hasHeaders, __defaultDelimiter, __defaultQuote, __defaultEscape, __defaultComment, true, __defaultBufferSize)
        { }

        public CSVReader(string filePath, bool hasHeaders, char delimiter)
            : this(new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), IOHelper.GetFileEncoding(filePath)), hasHeaders, delimiter, __defaultQuote, __defaultEscape, __defaultComment, true, __defaultBufferSize)
        { }

        public CSVReader(TextReader reader, bool hasHeaders, char delimiter)
            : this(reader, hasHeaders, delimiter, __defaultQuote, __defaultEscape, __defaultComment, true, __defaultBufferSize)
        { }

        public CSVReader(string filePath, bool hasHeaders, char delimiter, char quote, char escape, char comment, bool trimSpaces)
            : this(new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), IOHelper.GetFileEncoding(filePath)), hasHeaders, delimiter, quote, escape, comment, trimSpaces, __defaultBufferSize)
        { }

        public CSVReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, bool trimSpaces)
            : this(reader, hasHeaders, delimiter, quote, escape, comment, trimSpaces, __defaultBufferSize)
        { }

        public CSVReader(TextReader reader, bool hasHeaders, char delimiter, char quote, char escape, char comment, bool trimSpaces, int bufferSize)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize");
            //
            _bufferSize = bufferSize;
            //
            StreamReader stream;
            if ((stream = reader as StreamReader) != null &&
                stream.BaseStream.CanSeek &&
                stream.BaseStream.Length > 0)
                _bufferSize = (int)Math.Min(bufferSize, stream.BaseStream.Length);

            _reader = reader;
            _delimiter = delimiter;
            _quote = quote;
            _escape = escape;
            _comment = comment;

            _hasHeaders = hasHeaders;
            _trimSpaces = trimSpaces;

            _currentRecordIndex = -1;
        }



        #region Events
        public event EventHandler<CSVParseErrorEventArgs> ParseError;
        private void OnParseError(CSVParseErrorEventArgs e)
        {
            if (ParseError != null)
                ParseError(this, e);
        }
        #endregion



        #region Properties
        public char Delimiter
        {
            get { return _delimiter; }
        }

        public char Quote
        {
            get { return _quote; }
        }

        public char Escape
        {
            get { return _escape; }
        }

        public char Comment
        {
            get { return _comment; }
        }

        public bool HasHeaders
        {
            get { return _hasHeaders; }
        }

        public bool TrimSpaces
        {
            get { return _trimSpaces; }
        }

        public int BufferSize
        {
            get { return _bufferSize; }
        }

        public long CurrentRecordIndex
        {
            get { return _currentRecordIndex; }
        }

        public ParseErrorAction ParseErrorAction
        {
            get { return _parseErrorAction; }
            set { _parseErrorAction = value; }
        }

        public MissingFieldAction MissingFieldAction
        {
            get { return _missingFieldAction; }
            set { _missingFieldAction = value; }
        }

        public bool ParseErrorFlag
        {
            get { return _parseErrorFlag; }
        }

        public bool MissingFieldFlag
        {
            get { return _missingFieldFlag; }
        }


        public string this[int record, string field]
        {
            get
            {
                EnsureInitialize();
                //
                MoveToRecord(record);
                //
                return this[field];
            }
        }

        public string this[int record, int field]
        {
            get
            {
                EnsureInitialize();
                //
                MoveToRecord(record);
                //
                return this[field];
            }
        }

        public string this[string field]
        {
            get
            {
                EnsureInitialize();
                //
                if (string.IsNullOrEmpty(field))
                    throw new ArgumentNullException("field");
                if (!_hasHeaders)
                    throw new InvalidOperationException(__noHeadersMessage);
                int index = GetFieldIndex(field);
                if (index < 0)
                    return null;
                //
                return this[index];
            }
        }

        public string this[int field]
        {
            get
            {
                EnsureInitialize();
                //
                return ReadField(field, false, false);
            }
        }
        #endregion



        #region public method GetFieldCount
        public int GetFieldCount()
        {
            EnsureInitialize();
            //
            return _fieldCount;
        }
        #endregion

        #region public method GetHeaders
        public string[] GetHeaders()
        {
            EnsureInitialize();
            //
            string[] fieldHeaders = new string[_headers.Length];
            Array.Copy(_headers, fieldHeaders, _headers.Length);
            //
            return fieldHeaders;
        }
        #endregion

        #region public method GetFieldIndex
        public int GetFieldIndex(string header)
        {
            EnsureInitialize();
            //
            int index;
            if (_headerIndexes != null && _headerIndexes.TryGetValue(header, out index))
                return index;
            //
            return -1;
        }
        #endregion

        #region public method CopyCurrentRecordTo
        public void CopyCurrentRecordTo(string[] array)
        {
            CopyCurrentRecordTo(array, 0);
        }

        public void CopyCurrentRecordTo(string[] array, int index)
        {
            EnsureInitialize();
            //
            if (array == null)
                throw new ArgumentNullException("array");
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException("index");
            if (_currentRecordIndex < 0)
                throw new InvalidOperationException(__noCurrentRecordMessage);
            if (array.Length - index < _fieldCount)
                throw new ArgumentException(__notEnoughSpaceInArrayMessage);

            for (int i = 0; i < _fieldCount; i++)
            {
                if (_parseErrorFlag)
                    array[index + i] = null;
                else
                    array[index + i] = this[i];
            }
        }
        #endregion

        #region public method GetCurrentRawData
        public string GetCurrentRawData()
        {
            if (_buffer != null && _bufferLength > 0)
                return new string(_buffer, 0, _bufferLength);
            else
                return string.Empty;
        }
        #endregion

        #region public method MoveToRecord
        public void MoveToRecord(long record)
        {
            if (record < 0)
                throw new ArgumentOutOfRangeException("record");
            if (record < _currentRecordIndex)
                throw new InvalidOperationException(__supportedForwardOnlyModeMessage);
            //
            long offset = record - _currentRecordIndex;
            if (offset > 0)
                do
                {
                    if (!ReadNextRecord())
                        throw new EndOfStreamException(string.Format(CultureInfo.InvariantCulture, __cannotReadRecordAtIndexMessage, _currentRecordIndex - offset));
                }
                while (--offset > 0);
        }
        #endregion

        #region public method ReadNextRecord
        public bool ReadNextRecord()
        {
            return ReadNextRecord(false, false);
        }
        #endregion



        #region private method EnsureInitialize
        private void EnsureInitialize()
        {
            if (!_initialized)
                this.ReadNextRecord(true, false);
        }
        #endregion

        #region private method ReadBuffer
        private bool ReadBuffer()
        {
            if (_eof)
                return false;
            //
            CheckDisposed();
            //
            _bufferLength = _reader.Read(_buffer, 0, _bufferSize);
            //
            if (_bufferLength > 0)
                return true;
            else
            {
                _eof = true;
                _buffer = null;
                //
                return false;
            }
        }
        #endregion

        #region private method IsWhiteSpace
        private bool IsWhiteSpace(char c)
        {
            // если в качестве разделителя используется пробел или tab
            if (c == _delimiter)
                return false;
            else
            {
                if (c <= '\x00ff')
                    return (c == ' ' || c == '\t');
                else
                    return (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.SpaceSeparator);
            }
        }
        #endregion

        #region private method IsNewLine
        private bool IsNewLine(char c)
        {
            if (c == '\n')
                return true;
            else if (c == '\r' && _delimiter != '\r')
                return true;
            else
                return false;
        }
        #endregion

        #region private method ParseNewLine
        private bool ParseNewLine(ref int pos)
        {
            // возможно, уже достигнут конец буфера.
            if (pos == _bufferLength)
            { // дочитаем
                pos = 0;
                if (!ReadBuffer())
                    return false;
            }
            //
            char c = _buffer[pos];

            // '\r' считается новой строкой, если он не является разделителем
            if (c == '\r' && _delimiter != '\r')
            {
                pos++;
                // если дальше '\n' -- пропустим
                if (pos < _bufferLength)
                {
                    if (_buffer[pos] == '\n')
                        pos++;
                }
                else
                {
                    if (ReadBuffer())
                    {
                        if (_buffer[0] == '\n')
                            pos = 1;
                        else
                            pos = 0;
                    }
                }
                // если вышли/достигли границы буфера -- продвинемся
                if (pos >= _bufferLength)
                {
                    ReadBuffer();
                    pos = 0;
                }
                //
                return true;
            }
            else if (c == '\n')
            {
                pos++;
                // если вышли/достигли границы буфера -- продвинемся
                if (pos >= _bufferLength)
                {
                    ReadBuffer();
                    pos = 0;
                }
                //
                return true;
            }
            //
            return false;
        }
        #endregion

        #region private method ReadField
        private string ReadField(int field, bool initializing, bool discardValue)
        {
            if (!initializing)
            {
                if (field < 0 || field >= _fieldCount)
                    throw new ArgumentOutOfRangeException("field");
                if (_currentRecordIndex < 0)
                    throw new InvalidOperationException(__noCurrentRecordMessage);
                //
                // если это поле уже есть -- вернем. иначе, если его в принципе нет -- обработка
                if (_fields[field] != null)
                    return _fields[field];
                else if (_missingFieldFlag)
                    return HandleMissingField(null, field, ref _nextFieldStart);
            }
            //
            CheckDisposed();
            int index = _nextFieldIndex;

            while (index < field + 1)
            {
                // если поле начинается с конца буфера
                if (_nextFieldStart == _bufferLength)
                {
                    _nextFieldStart = 0;
                    // дочитаем
                    ReadBuffer();
                }
                //
                string value = null;
                if (_missingFieldFlag)
                    value = HandleMissingField(value, index, ref _nextFieldStart);
                else if (_nextFieldStart == _bufferLength)
                {
                    // если текущее поле -- запрашиваемое, то значение поля = "" ("f1,f2,f3,")
                    // иначе -- битый CSV
                    if (index == field)
                    {
                        if (!discardValue)
                        {
                            value = string.Empty;
                            _fields[index] = value;
                        }
                    }
                    else
                    {
                        value = HandleMissingField(value, index, ref _nextFieldStart);
                    }
                }
                else
                {
                    // удалим пробелы из начала (если надо)
                    if (_trimSpaces)
                        SkipWhiteSpaces(ref _nextFieldStart);
                    if (_eof)
                        value = string.Empty;
                    else 
                    {
                        // поле не в quote'е
                        int start = _nextFieldStart;
                        int pos = _nextFieldStart;
                        while (true)
                        {
                            while (pos < _bufferLength)
                            {
                                char c = _buffer[pos];

                                if (c == _delimiter)
                                {
                                    _nextFieldStart = pos + 1;
                                    break;
                                }
                                else if (c == '\r' || c == '\n')
                                {
                                    _nextFieldStart = pos;
                                    _eol = true;
                                    break;
                                }
                                else
                                    pos++;
                            }
                            //
                            if (pos < _bufferLength)
                                break;
                            else
                            {
                                if (!discardValue)
                                    value += new string(_buffer, start, pos - start);
                                //
                                start = 0;
                                pos = 0;
                                _nextFieldStart = 0;
                                //
                                if (!ReadBuffer())
                                    break;
                            }
                        }
                        //
                        if (!discardValue)
                        {
                            // удалим пробелы с конца (если надо)
                            if (_trimSpaces)
                            {
                                if (!_eof && pos > start)
                                {
                                    pos--;
                                    while (pos > -1 && IsWhiteSpace(_buffer[pos]))
                                        pos--;
                                    pos++;
                                    //
                                    if (pos > 0)
                                        value += new string(_buffer, start, pos - start);
                                }
                                else
                                    pos = -1;

                                // если pos <= 0, это означает что мы дошли до начала буфера и надо тримнуть предыдущее 
                                // значение
                                if (pos <= 0)
                                {
                                    pos = (value == null ? -1 : value.Length - 1);
                                    // тримаем
                                    while (pos > -1 && IsWhiteSpace(value[pos]))
                                        pos--;
                                    pos++;
                                    //
                                    if (pos > 0 && pos != value.Length)
                                        value = value.Substring(0, pos);
                                }
                            }
                            else
                            {
                                if (!_eof && pos > start)
                                    value += new string(_buffer, start, pos - start);
                            }
                            //
                            if (value == null)
                                value = string.Empty;
                        }

                        if (_eol || _eof)
                        {
                            _eol = ParseNewLine(ref _nextFieldStart);

                            // если reader инициализируется или мы работаем с последним полем, то конец строки -- нормально
                            if (!initializing && index != _fieldCount - 1)
                            {
                                if (value != null && value.Length == 0)
                                    value = null;

                                value = HandleMissingField(value, index, ref _nextFieldStart);
                            }
                        }
                        //
                        if (!String.IsNullOrEmpty(value))
                        {
                            var len = value.Length;
                            if (len >= 2)
                            {
                                if (value[0] == _quote && value[len - 1] == _quote)
                                    value = value.Substring(1, len - 2);
                                if (String.IsNullOrWhiteSpace(value))
                                    value = String.Empty;
                            }
                        }
                        //
                        if (!discardValue)
                            _fields[index] = value;
                    }
                }
                //
                _nextFieldIndex = Math.Max(index + 1, _nextFieldIndex);
                //
                if (index == field)
                {
                    // если выполняется инициализация -- вернем null (инициализация завершена)
                    if (initializing)
                    {
                        if (_eol || _eof)
                            return null;
                        else
                            return string.IsNullOrEmpty(value) ? string.Empty : value;
                    }
                    else
                        return value;
                }
                //
                index++;
            }

            // скверная примета :(
            HandleParseError(new CSVReaderException(GetCurrentRawData(), _nextFieldStart, Math.Max(0, _currentRecordIndex), index), ref _nextFieldStart);
            return null;
        }
        #endregion

        #region private method ReadNextRecord
        private bool ReadNextRecord(bool onlyReadHeaders, bool skipToNextLine)
        {
            if (_eof)
            {
                if (_firstRecordInCache)
                {
                    _firstRecordInCache = false;
                    _currentRecordIndex++;

                    return true;
                }
                else
                    return false;
            }
            //
            CheckDisposed();
            //
            if (!_initialized)
            {
                _buffer = new char[_bufferSize];

                // если header'ы будут получены -- перезатрется
                _headers = new string[0];

                if (!ReadBuffer())
                    return false;

                if (!SkipEmptyAndCommentedLines(ref _nextFieldStart))
                    return false;

                // вычисляем число полей
                _fieldCount = 0;
                _fields = new string[16];

                while (ReadField(_fieldCount, true, false) != null)
                {
                    if (_parseErrorFlag)
                    {
                        _fieldCount = 0;
                        Array.Clear(_fields, 0, _fields.Length);
                        _parseErrorFlag = false;
                        _nextFieldIndex = 0;
                    }
                    else
                    {
                        _fieldCount++;

                        if (_fieldCount == _fields.Length)
                            Array.Resize<string>(ref _fields, (_fieldCount + 1) * 2);
                    }
                }
                //
                // увеличим на 1, т.к. должно быть общее число полей, а не индекс последнего
                _fieldCount++;
                //
                if (_fields.Length != _fieldCount)
                    Array.Resize<string>(ref _fields, _fieldCount);
                _initialized = true;

                // если есть header'ы, прочтем заодно и следующую строку
                if (_hasHeaders)
                {
                    // первую строку не считаем, т.к. это были header'ы.
                    _currentRecordIndex = -1;
                    _firstRecordInCache = false;
                    _headers = new string[_fieldCount];
                    _headerIndexes = new Dictionary<string, int>(_fieldCount, __headerComparer);
                    //
                    for (int i = 0; i < _fields.Length; i++)
                    {
                        _headers[i] = _fields[i];
                        _headerIndexes.Add(_fields[i], i);
                    }
                    //
                    // к первой записи
                    if (!onlyReadHeaders)
                    {
                        if (!SkipEmptyAndCommentedLines(ref _nextFieldStart))
                            return false;

                        Array.Clear(_fields, 0, _fields.Length);
                        _nextFieldIndex = 0;
                        _eol = false;

                        _currentRecordIndex++;
                        return true;
                    }
                }
                else
                {
                    if (onlyReadHeaders)
                    {
                        _firstRecordInCache = true;
                        _currentRecordIndex = -1;
                    }
                    else
                    {
                        _firstRecordInCache = false;
                        _currentRecordIndex = 0;
                    }
                }
            }
            else
            {
                if (skipToNextLine)
                    SkipToNextLine(ref _nextFieldStart);
                else if (_currentRecordIndex > -1 && !_missingFieldFlag)
                {
                    if (!_eol && !_eof)
                    {
                        while (ReadField(_nextFieldIndex, true, true) != null)
                        { }
                    }
                }
                //
                if (!_firstRecordInCache && !SkipEmptyAndCommentedLines(ref _nextFieldStart))
                    return false;
                //
                if (_hasHeaders || !_firstRecordInCache)
                    _eol = false;
                //
                if (_firstRecordInCache)
                    _firstRecordInCache = false;
                else
                {
                    Array.Clear(_fields, 0, _fields.Length);
                    _nextFieldIndex = 0;
                }
                //
                _missingFieldFlag = false;
                _parseErrorFlag = false;
                _currentRecordIndex++;
            }
            //
            return true;
        }
        #endregion

        #region private method SkipEmptyAndCommentedLines
        private bool SkipEmptyAndCommentedLines(ref int pos)
        {
            if (pos < _bufferLength)
                DoSkipEmptyAndCommentedLines(ref pos);
            //
            while (pos >= _bufferLength && !_eof)
            {
                if (ReadBuffer())
                {
                    pos = 0;
                    DoSkipEmptyAndCommentedLines(ref pos);
                }
                else
                    return false;
            }
            //
            return !_eof;
        }
        #endregion

        #region private method DoSkipEmptyAndCommentedLines
        private void DoSkipEmptyAndCommentedLines(ref int pos)
        {
            while (pos < _bufferLength)
            {
                if (_buffer[pos] == _comment)
                {
                    pos++;
                    SkipToNextLine(ref pos);
                }
                else if (ParseNewLine(ref pos))
                    continue;
                else
                    break;
            }
        }
        #endregion

        #region private method SkipWhiteSpaces
        private bool SkipWhiteSpaces(ref int pos)
        {
            while (true)
            {
                while (pos < _bufferLength && IsWhiteSpace(_buffer[pos]))
                    pos++;
                //
                if (pos < _bufferLength)
                    break;
                else
                {
                    pos = 0;
                    if (!ReadBuffer())
                        return false;
                }
            }
            //
            return true;
        }
        #endregion

        #region private method SkipToNextLine
        private bool SkipToNextLine(ref int pos)
        {
            // ((pos = 0) == 0) -- сброс position inline
            while ((pos < _bufferLength || (ReadBuffer() && ((pos = 0) == 0))) && !ParseNewLine(ref pos))
                pos++;

            return !_eof;
        }
        #endregion

        #region private method HandleParseError
        private void HandleParseError(CSVReaderException error, ref int pos)
        {
            if (error == null)
                throw new ArgumentNullException("error");
            //
            _parseErrorFlag = true;
            //
            switch (_parseErrorAction)
            {
                case ParseErrorAction.ThrowException:
                    throw error;
                case ParseErrorAction.RaiseEvent:
                    CSVParseErrorEventArgs e = new CSVParseErrorEventArgs(error, ParseErrorAction.ThrowException);
                    OnParseError(e);
                    //
                    switch (e.Action)
                    {
                        case ParseErrorAction.ThrowException:
                            throw e.Exception;
                        case ParseErrorAction.RaiseEvent:
                            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, __parseErrorActionInvalidInsideParseErrorEventMessage, e.Action), e.Exception);
                        case ParseErrorAction.AdvanceToNextLine:
                            // если уже в конце строки -- не фиг пропускать
                            if (!_missingFieldFlag && pos >= 0)
                                SkipToNextLine(ref pos);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                    break;
                case ParseErrorAction.AdvanceToNextLine:
                    // если уже в конце строки -- не фиг пропускать
                    if (!_missingFieldFlag && pos >= 0)
                        SkipToNextLine(ref pos);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
        #endregion

        #region private method HandleMissingField
        private string HandleMissingField(string value, int fieldIndex, ref int currentPosition)
        {
            if (fieldIndex < 0 || fieldIndex >= _fieldCount)
                throw new ArgumentOutOfRangeException("fieldIndex");

            _missingFieldFlag = true;

            for (int i = fieldIndex + 1; i < _fieldCount; i++)
                _fields[i] = null;

            if (value != null)
                return value;
            else
            {
                switch (_missingFieldAction)
                {
                    case MissingFieldAction.ThrowException:
                        HandleParseError(new CSVReaderException(GetCurrentRawData(), currentPosition, Math.Max(0, _currentRecordIndex), fieldIndex), ref currentPosition);
                        return value;
                    case MissingFieldAction.ReplaceByEmpty:
                        return string.Empty;
                    case MissingFieldAction.ReplaceByNull:
                        return null;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
        #endregion



        #region interface IEnumerable<string[]>
        public CSVReader.RecordEnumerator GetEnumerator()
        {
            return new CSVReader.RecordEnumerator(this);
        }

        IEnumerator<string[]> IEnumerable<string[]>.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region interface IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region interface IDisposable
        private bool _isDisposed = false;
        private readonly object _lock = new object();



        public event EventHandler Disposed;
        private void OnDisposed(System.EventArgs e)
        {
            if (Disposed != null)
                Disposed(this, e);
        }


        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        private void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(this.GetType().FullName);
        }

        public override void Dispose()
        {
            if (!_isDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                try
                {
                    if (disposing && _reader != null)
                        lock (_lock)
                            if (_reader != null)
                            {
                                _reader.Dispose();

                                _reader = null;
                                _buffer = null;
                                _eof = true;
                            }
                }
                finally
                {
                    _isDisposed = true;
                    try
                    {
                        OnDisposed(System.EventArgs.Empty);
                    }
                    catch { }
                }
            }
        }

        ~CSVReader()
        {
            Dispose(false);
        }
        #endregion



        public sealed class RecordEnumerator : IEnumerator<string[]>, IEnumerator
        {
            private CSVReader _reader;
            private string[] _current;
            private long _currentRecordIndex;



            public RecordEnumerator(CSVReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException("reader");

                _reader = reader;
                _current = null;

                _currentRecordIndex = reader._currentRecordIndex;
            }



            #region private method ValidateState
            private void ValidateState()
            {
                if (_reader._currentRecordIndex != _currentRecordIndex)
                    throw new InvalidOperationException(__enumeratorStateIsInvalidMessage);
            }
            #endregion



            #region interface IEnumerator<string[]>
            public string[] Current
            {
                get { return _current; }
            }

            public bool MoveNext()
            {
                ValidateState();
                //
                if (_reader.ReadNextRecord())
                {
                    _current = new string[_reader._fieldCount];

                    _reader.CopyCurrentRecordTo(_current);
                    _currentRecordIndex = _reader._currentRecordIndex;
                    //
                    return true;
                }
                else
                {
                    _current = null;
                    _currentRecordIndex = _reader._currentRecordIndex;
                    //
                    return false;
                }
            }
            #endregion

            #region interface IEnumerator
            public void Reset()
            {
                ValidateState();
                //
                _reader.MoveToRecord(-1);
                _current = null;
                _currentRecordIndex = _reader._currentRecordIndex;
            }

            object IEnumerator.Current
            {
                get
                {
                    ValidateState();
                    //
                    return this.Current;
                }
            }
            #endregion

            #region interfacw IDisposable
            public void Dispose()
            {
                _reader = null;
                _current = null;
            }
            #endregion
        }
    }
}
