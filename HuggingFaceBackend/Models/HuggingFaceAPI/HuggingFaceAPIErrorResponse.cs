using Newtonsoft.Json;

namespace HuggingFaceBackend.Models.HuggingFaceAPI
{
    /// <summary>
    /// Модель ошибки от Hugging Face API
    /// </summary>
    public class HuggingFaceAPIErrorResponse
    {
        /// <summary>
        /// Ошибка
        /// </summary>
        public string? Error { get; set; } = null;

        /// <summary>
        /// Оставшееся время 
        /// </summary>
        [JsonProperty(PropertyName = "estimated_time")]
        public decimal EstimatedTime { get; set; }
    }
}
