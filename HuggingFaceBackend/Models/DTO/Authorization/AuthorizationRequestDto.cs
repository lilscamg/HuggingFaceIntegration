namespace HuggingFaceBackend.Models.DTO.Authorization
{
    /// <summary>
    /// ДТО-модель запроса для авторизации
    /// </summary>
    public class AuthorizationRequestDto
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Хэшированный пароль
        /// </summary>
        public string HashedPassword { get; set; }
    }
}
