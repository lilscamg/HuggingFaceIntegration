using HuggingFaceBackend.Models.DTO.HuggingFaceAPI;
using HuggingFaceBackend.Models.HuggingFaceAPI;
using HuggingFaceBackend.Models.Interfaces;
using HuggingFaceBackend.Models.Response;

namespace HuggingFaceBackend.Services
{
    /// <summary>
    /// Интерфейс сервиса для взаимодействия с API Hugging Face
    /// </summary>
    public interface IHuggingFaceApiClient
    {
        /// <summary>
        /// Получить результат использования модели Hugging Face
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Результат</returns>
        public Task<IResponse?> GetResult(HuggingFaceAPIRequestDto request);
    }
}
