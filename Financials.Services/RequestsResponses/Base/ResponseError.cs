namespace Financials.Services.RequestsResponses.Base
{
    public class ResponseError
    {
        public ResponseError(ResponseErrorType type, string customMessage, string internalError = null)
        {
            Type = type;
            CustomMessage = customMessage;
            InternalError = internalError;
        }

        public ResponseErrorType Type { get; set; }

        public string CustomMessage { get; set; }
        public string InternalError { get; set; }
    }
}
