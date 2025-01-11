using HuggingFaceBackend.Models.Interfaces;

namespace HuggingFaceBackend.Models.Response
{
    public class Response<TData> : IResponse
        where TData : class
    {
        public TData? Data { get; set; }

        public bool IsSuccess { get; } = true;
    }
}
