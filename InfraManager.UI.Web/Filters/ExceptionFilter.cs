using System;
using Inframanager;
using Inframanager.BLL;
using InfraManager.BLL;
using InfraManager.BLL.Localization;
using InfraManager.ResourcesArea;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using IM.Core.HttpInfrastructure;
using Inframanager.BLL.Restrictions;
using AccessDeniedException = InfraManager.BLL.AccessDeniedException;

namespace InfraManager.UI.Web.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;
        private readonly ILocalizeText _localizer;

        public ExceptionFilter(ILogger<ExceptionFilter> logger,
         ILocalizeText localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var userID = "No ID";
            try //TODO: Убрать весь блок
            {
                userID = context.HttpContext.GetUserId().ToString();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while getting current User ID, potentially not authenticated user");
            }


            if (context.Exception is AccessDeniedException accessDenied)
            {
                _logger.LogWarning($"{accessDenied.Operation}. User ID = {userID}");
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
            else if (context.Exception is UnauthorizedAccessException unauthorizedAccessException) //TODO: Убрать этот блок. Unauthorized фильтруются на стороне MIddleware
            {
                _logger.LogWarning($"{unauthorizedAccessException.Message} User ID = {userID}");
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden; // Для справки это не Unauthorized, это Not Authenticated и это 401 код
            }
            else if (context.Exception is ObjectNotFoundException notFound)
            {
                _logger.LogWarning(notFound.Message);
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            else if (context.Exception is InvalidObjectException invalidObject)
            {
                _logger.LogInformation(
                    $"Validation error at {context.HttpContext.Request.Path} from user (ID = {userID}). {invalidObject.Message}");
                context.Result = new ObjectResult(invalidObject.Message)
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };// тот случай, когда текст исключения отдается клиенту
            }
            else if (context.Exception is UniqueKeyConstraintViolationException uniqueConstraintViolation)
            {
                _logger.LogInformation(
                    $"Unique constraint violation at {context.HttpContext.Request.Path} from user (ID = {userID}): {uniqueConstraintViolation.Keys}");
                context.Result = new ObjectResult($"NONUNIQUE {uniqueConstraintViolation.Keys}")
                { 
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };
            }
            else if (context.Exception is ForeignKeyConstraintViolationException foreignConstraintViolation)
            {
                _logger.LogInformation(
                    $"Unique constraint violation at {context.HttpContext.Request.Path} from user (ID = {userID}): {foreignConstraintViolation.Keys}");
                context.Result = new ObjectResult($"NONEXISTING {foreignConstraintViolation.Keys}")
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };
            }
            else if (context.Exception is TaskCanceledException)
            {
                context.Result = new ObjectResult("Request was cancelled")
                {
                    StatusCode = StatusCodes.Status202Accepted
                };
            }
            else if (context.Exception is HttpException httpException)
            {
                _logger.LogError(httpException, $"HttpException ресурса {context.HttpContext.Request.Path}");
                context.HttpContext.Response.StatusCode = httpException.HttpStatusCode;
            }
            else if (context.Exception is ObjectReadonlyException readonlyException)
            {
                _logger.LogInformation(
                    $"{context.Exception.Message} ID пользователя = {userID}.");
                context.Result = new StatusCodeResult((int)StatusCodes.Status405MethodNotAllowed);
            }
            else if (context.Exception is ConcurrencyException)
            {
                context.Result = new StatusCodeResult((int)StatusCodes.Status409Conflict);
            }
            else if (context.Exception is NotModifiedException notModifiedException)
            {
                _logger.LogTrace($"{nameof(NotModifiedException)}: {notModifiedException.Message}");
                context.Result = new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            else if (context.Exception is WebApiException webApiException)
            {
                _logger.LogInformation(
                    $"Workflow api error {webApiException?.Message}");
                context.Result = new ObjectResult(_localizer.Localize(nameof(Resources.WorkflowServiceUnavailable)))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            else if (context.Exception is SchedulerApiException exception)
            {
                _logger.LogInformation(
                    $"Scheduler api error {Resources.CantDeleteTaskImport}");
                context.Result = new ObjectResult(_localizer.Localize(nameof(Resources.CantDeleteTaskImport)))
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            else if (context.Exception is RestrictionException restrictionException)
            {
                _logger.LogInformation(
                    restrictionException.Message);
                context.Result = new ObjectResult("Ограничения текущей версии")
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };
            }
            else
            {
                _logger.LogError(context.Exception, "Unexpected error at {Path}",context.HttpContext.Request.Path);
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            context.ExceptionHandled = true;
        }
    }
}
