namespace HuggingFaceBackend.Models.DTO
{
    /// <summary>
    /// Пользователь системы
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

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
