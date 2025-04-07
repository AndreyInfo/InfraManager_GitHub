using InfraManager.BLL.FieldEdit;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using InfraManager.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.CrossPlatform.WebApi.Controllers
{
    [Authorize]
    public class FieldEditController : ControllerBase
    {
        private readonly IEntityEditorProvider _entityEditorProvider;
        public FieldEditController(IEntityEditorProvider entityEditorProvider)
        {
            _entityEditorProvider = entityEditorProvider ?? throw new ArgumentNullException(nameof(entityEditorProvider));
        }

        [HttpPost]
        [Route("elp/set-field")]
        [Route("licence-scheme/set-field")]
        [Route("SoftwareInstallations/set-field")]
        public async Task<IActionResult> SetFieldAsync([FromForm] InfraManager.Web.Models.SetFieldRequestUI request, CancellationToken cancellationToken = default)
        {

            SetFieldRequest req = new SetFieldRequest()
            {
                ID = request.ID,
                ObjClassID = request.ObjClassID,
                ClassID = request.ClassID,
                FieldValue = new FieldValueModel()
                {
                    Field = request.Field,
                    NewValue = request.NewValue,
                    OldValue = request.OldValue,
                    ReplaceAnyway = request.ReplaceAnyway,
                },
            };

            BaseResult<SetFieldResult, BaseError> setFieldResponse;

            var editor = _entityEditorProvider.GetEntityEditor(req);
            if (editor == null)
                setFieldResponse = new BaseResult<SetFieldResult, BaseError>(null, BaseError.BadParamsError);
            else
                setFieldResponse = await editor.HandleAsync(req, cancellationToken);

            return Ok(ConvertToResponseUI(setFieldResponse));
        }

        private SetFieldResponse ConvertToResponseUI(BaseResult<SetFieldResult, BaseError> responseCrossPlaform)
        {
            return new SetFieldResponse()
            {
                ResultWithMessage = new Web.Models.ResultWithMessage()
                {
                    Result = responseCrossPlaform.Success ? 0 : (int)responseCrossPlaform.Fault.Value,
                    Message = responseCrossPlaform.Result?.Message
                }
            };
        }
    }
}
