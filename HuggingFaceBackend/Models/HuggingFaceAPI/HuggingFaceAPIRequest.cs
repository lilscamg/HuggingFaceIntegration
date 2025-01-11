using Newtonsoft.Json;

namespace HuggingFaceBackend.Models.HuggingFaceAPI
{
    /// <summary>
    /// Модель запроса к Hugging Face API
    /// </summary>
    public class HuggingFaceAPIRequest
    {
        /// <summary>
        /// Данные для обработки
        /// </summary>
        [JsonProperty(PropertyName = "inputs")]
        public required string Inputs { get; set; }
    }
}
