namespace Financials.Services.RequestsResponses.Base
{
    public enum ResponseErrorType
    {
        Success = 100,
        ValidationError = 400,
        NotFound = 404,
        InternalError = 500,
        Forbidden = 403
    }
}
