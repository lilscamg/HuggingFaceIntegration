using Newtonsoft.Json;

namespace HuggingFaceBackend.Models.HuggingFaceAPI
{
    /// <summary>
    /// Модель ответа от Hugging Face API по задаче 
    /// </summary>
    public class HuggingFaceAPITextSummarizeResponse
    {
        /// <summary>
        /// Метка
        /// </summary>
        [JsonProperty(PropertyName = "summary_text")]
        public required string SummaryText { get; set; }
    }
}
