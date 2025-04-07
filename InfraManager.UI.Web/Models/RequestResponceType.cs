using System;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.Web.Controllers
{
    //ТЫСЯЧУ РАЗ ПОДУМАЙ ПРЕЖДЕ ЧЕМ РЕДАКТИРОВАТЬ ЭТО ПЕРЕЧИСЛЕНИЕ
    public enum RequestResponceType
    {
        Success = 0,
        NullParamsError = 1,
        BadParamsError = 2,
        AccessError = 3,
        GlobalError = 4,
        ConcurrencyError = 5,
        ObjectDeleted = 6,
        OperationError = 7,
        ValidationError = 8,
        FiltrationError = 9, //for tableList
        NegotiationEnded = 10, //for negotiation vote methods
        Timeout = 11,
        HasDuplicates = 12,//for Device InventoryNumber, SerialNumber and Code 
        ExistsByName = 13,//для справочников
        SynonymNotEnabled = 14 //для синонимов
    }

    public class ResultWithMessage
    {
        protected ResultWithMessage()
        { }

        public RequestResponceType Result { get; protected set; }
        public string Message { get; protected set; }

        public bool IsResult { get; protected set; }

        public static ResultWithMessage Create(RequestResponceType type)
        {
            return ResultWithMessage.Create(type, string.Empty);
        }

        public static ResultWithMessage Create(RequestResponceType type, string message, bool result = false)
        {
            var retval = new ResultWithMessage();
            retval.Result = type;
            retval.Message = message;
            retval.IsResult = result;
            //
            return retval;
        }
    }

    [Obsolete("Try not to use")]
    public class ResultData<T> /*where T : class*/
    {
        public RequestResponceType Result { get; set; }
        public T Data { get; set; }

        public ResultData(RequestResponceType result, T data)
        {
            this.Result = result;
            this.Data = data;
        }

        public static ResultData<T> Create(RequestResponceType type)
        {
            return new ResultData<T>(type, default);
        }

        public static ResultData<T> Create(RequestResponceType type, T data)
        {
            return new ResultData<T>(type, data);
        }

        public static ResultData<T> FromBaseResult(BaseResult<T, BaseError> baseResult)
        {
            if (baseResult.Success)
                return new ResultData<T>(RequestResponceType.Success, baseResult.Result);

            return new ResultData<T>((RequestResponceType)(int)baseResult.Fault, default);
        }

        public static ResultData<T> FromError(BaseError baseError)
        {
            return new ResultData<T>((RequestResponceType)(int)baseError, default);
        }
    }

    public class InsertResultData<TKey> where TKey : struct
    {
        private InsertResultData(TKey id, RequestResponceType result)
        {
            ID = id;
            Result = result;
        }

        public TKey ID { get; }
        public RequestResponceType Result { get; }

        public static InsertResultData<TKey> Success(TKey id)
        {
            return new InsertResultData<TKey>(id, RequestResponceType.Success);
        }

        public static InsertResultData<TKey> Failure(RequestResponceType result)
        {
            return new InsertResultData<TKey>(default(TKey), result);
        }
    }
}