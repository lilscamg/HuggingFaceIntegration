using HuggingFaceBackend.Enums;
using System.Text.Json.Serialization;

namespace HuggingFaceBackend.Models.DTO.HuggingFaceAPI
{
    /// <summary>
    /// ДТО-модель запроса к серверу для взаимодействия с Hugging Face API
    /// </summary>
    public class HuggingFaceAPIRequestDto
    {
        /// <summary>
        /// Название модели
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Данные для обработки
        /// </summary>
        public string Inputs { get; set; }

        /// <summary>
        /// Тип задачи
        /// </summary>
        public TaskType TaskType { get; set; }
    }
}
