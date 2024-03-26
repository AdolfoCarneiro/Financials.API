using Financials.Services.RequestsResponses.Base;
using Microsoft.AspNetCore.Mvc;

namespace Financials.API
{
    public static class ControllerExtension
    {
        public static ActionResult GetResponse<T>(this ControllerBase controller, ApplicationResponse<T> response) where T : class
        {
            if (response.Valid)
                return controller.Ok(response);
            else
            {
                switch (response.Error.Type)
                {
                    case ResponseErrorType.ValidationError:
                        return controller.BadRequest(response);
                    case ResponseErrorType.InternalError:
                        return controller.StatusCode(StatusCodes.Status500InternalServerError, response);
                    case ResponseErrorType.Forbidden:
                        return controller.StatusCode(StatusCodes.Status403Forbidden, response);
                    case ResponseErrorType.NotFound:
                        return controller.StatusCode(StatusCodes.Status404NotFound, response);
                    default: return controller.Ok();
                }
            }
        }
    }
}
