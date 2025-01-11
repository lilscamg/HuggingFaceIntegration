namespace HuggingFaceBackend.Models.Interfaces
{
    /// <summary>
    /// Интерфейс ответов от API
    /// </summary>
    public interface IResponse 
    {
        public bool IsSuccess { get; }
    }
}
