using HuggingFaceBackend.Models.Interfaces;

namespace HuggingFaceBackend.Models.Response
{
    /// <summary>
    /// Модель ответа с ошибкой от API
    /// </summary>
    public class ErrorResponse : IResponse
    {
        public ErrorResponse(string error)
        {
            this.Error = error;
        }

        public string Error { get; set; }
        public bool IsSuccess { get; } = false;
    }
}
