using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using InfraManager.Web.BLL.Settings;
using InfraManager.Web.Models;

namespace InfraManager.Web.Controllers
{
    /// <summary>
    /// Базовый контроллер для документов получаемых через CrossPlatform
    /// </summary>
    public abstract class BaseObjectCrossPlatformController : BaseApiController
    {
        /// <summary>
        /// Идентификатор класса объекта
        /// </summary>
        protected abstract int ObjClassID { get; }

        /// <summary>
        /// Клиент для вызова нового api кросплатформы для работы с сохранением конкретных полей
        /// </summary>
        //private readonly Lazy<IFieldEditService> _fieldEditService;

        /// <summary>
        /// Конфигурация
        /// </summary>
        protected ConfigurationSet configuration;

        /// <summary>
        /// Клиент для вызова нового api кросплатформы для работы с историей
        /// </summary>
        //private readonly Lazy<IHistoryService> _historyService;


        public BaseObjectCrossPlatformController()
        {
        }

        /// <summary>
        /// Конвертируем запрос UI на сохранение изменений в запрос для CrossPlatform
        /// </summary>
        /// <param name="request">запрос со сторны UI <see cref = "SetFieldRequestUI"/> </param>
        /// <returns> запрос для CrossPlatform  <see cref = "SetFieldRequest"/></returns>
        private SetFieldRequest ConvertToRequest(SetFieldRequestUI request)
        {
            return new SetFieldRequest()
            {
                ID = request.ID,
                ObjClassID = ObjClassID,
                FieldValue = new FieldValueModel()
                {
                    Field = request.Field,
                    NewValue = request.NewValue,
                    OldValue = request.OldValue,
                    ReplaceAnyway = request.ReplaceAnyway

                }
            };
        }

        /// <summary>
        /// Конвертируем ответ CrossPlatform на изменение поля в response для UI
        /// </summary>
        /// <param name="responseCrossPlaform"> ответ CrossPlatform <see cref = "BaseResult{SetFieldResult, GeneralFualts}"/> </param>
        /// <returns>response для UI <see cref = "SetFieldResponse"/>  </returns>
        private SetFieldResponse ConvertToResponseUI(BaseResult<SetFieldResult, BaseError> responseCrossPlaform)
        {
            return new SetFieldResponse()
            {
                ResultWithMessage = new Web.Models.ResultWithMessage()
                {
                    Result = responseCrossPlaform.Success ? 0 : (int)responseCrossPlaform.Fault.Value,
                    Message = responseCrossPlaform.Result.Message
                }
            };
        }
    }
}