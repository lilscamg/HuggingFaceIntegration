namespace HuggingFaceBackend.Models.DTO.Registration
{
    /// <summary>
    /// ДТО-модель запроса для регистрации
    /// </summary>
    public class RegistrationRequestDto
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Хэшированный пароль
        /// </summary>
        public string HashedPassword { get; set; }

        /// <summary>
        /// Хэшированный повторный пароль для проверки
        /// </summary>
        public string HashedCheckPassword { get; set; }
    }
}
