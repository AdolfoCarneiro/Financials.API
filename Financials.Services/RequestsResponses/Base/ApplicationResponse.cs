using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Financials.Services.RequestsResponses.Base
{
    public sealed class ApplicationResponse<T> where T : class
    {
        public bool Valid
        {
            get { return this.Error == null; }
        }
        public ResponseError Error { get; private set; }

        public T Data { get; set; } = null;
        public void AddError(List<ValidationFailure> fluentValidations)
        {
            var errorValidation = fluentValidations.Select(f => f.ErrorMessage);

            this.AddError(ResponseErrorType.ValidationError, string.Join(';', errorValidation));
        }

        public void AddError(List<IdentityError> identityErrors)
        {
            var errorValidation = identityErrors.FirstOrDefault();
            this.AddError(ResponseErrorType.ValidationError, string.Join(';', errorValidation?.Description));
        }
        public void AddError(Exception exception)
        {
            this.Error ??= new ResponseError(ResponseErrorType.InternalError, "Erro interno", exception.Message);
        }

        public void AddError(Exception exception, string customMessage)
        {
            this.Error ??= new ResponseError(ResponseErrorType.InternalError, customMessage, exception.Message);
        }

        public void AddError(ResponseErrorType type, string customMessage)
        {
            this.Error ??= new ResponseError(type, customMessage);
        }

        public void AddData(T data) {  this.Data = data; }
    }
}
