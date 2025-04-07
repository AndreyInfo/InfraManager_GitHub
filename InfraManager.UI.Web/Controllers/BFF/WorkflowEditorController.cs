using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfraManager.BLL;
using InfraManager.BLL.SD.ExpressionEditor;
using InfraManager.BLL.SD.ExpressionEditor.ObjectEditors;
using InfraManager.IM.BusinessLayer.Messages;
using InfraManager.UI.Web.Models.WorkflowEditor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InfraManager.SD.BusinessLayer.Calls;
using InfraManager.SD.BusinessLayer.WorkOrders;
using InfraManager.Web.BLL.SD.RFC;

namespace InfraManager.UI.Web.Controllers.BFF;

[Authorize]
[ApiController]
[Route("bff/{controller}")]
public class WorkflowEditorController : ControllerBase
{
    private readonly EditorExpressionManager _expressionEditorManager;
    
    public WorkflowEditorController()
    {
        _expressionEditorManager = new EditorExpressionManager();
    }


    [HttpGet("Encodings")]
    public EncodingModel[] GetEncodings()
    {
        var encodings = Encoding.GetEncodings().ToArray();
        return encodings.Select(x => new EncodingModel { Name = x.Name, DisplayName = x.DisplayName }).ToArray();
    }

    [HttpGet("EditorObjects")]
    public ExpressionEditorObjects[] ExpressionEditorObjects()
    {
        return _expressionEditorManager.GetAllObjects();
    }

    [HttpGet("EditorObjects/{objectName}")]
    public EditorObjectInformation ExpressionEditorObjectInformation([FromRoute] string objectName)
    {
        return _expressionEditorManager.GetObjectInformation(objectName);
    }


    public class NextAvailableOperationsRequest
    {
        public string ObjectName { get; set; }
        public string StartPropertyName { get; set; } = "";
    }

    [HttpGet("EditorObjects/AvailableProperties")]
    public EditorObjectInformation GetAvailableNextOperations([FromQuery] NextAvailableOperationsRequest request)
    {
        if (request.StartPropertyName == null)
        {
            return new EditorObjectInformation()
            {
                Classes = _expressionEditorManager.GetAllObjects(request.ObjectName)
            };
        }

        return _expressionEditorManager.GetAvailableNextOperations(request.ObjectName, request.StartPropertyName);
    }
}