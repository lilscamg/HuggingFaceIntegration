namespace HuggingFaceBackend.Models.HuggingFaceAPI
{
    /// <summary>
    /// Модель ответа от Hugging Face API
    /// </summary>
    public class HuggingFaceAPITextClassificationResponse
    {
        /// <summary>
        /// Метка
        /// </summary>
        public required string Label { get; set; }

        /// <summary>
        /// Значение оценки
        /// </summary>
        public double Score { get; set; }
    }
}
