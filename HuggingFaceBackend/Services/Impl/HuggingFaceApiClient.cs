using HuggingFaceBackend.Enums;
using HuggingFaceBackend.Models.ConfigSections;
using HuggingFaceBackend.Models.DTO.HuggingFaceAPI;
using HuggingFaceBackend.Models.HuggingFaceAPI;
using HuggingFaceBackend.Models.Interfaces;
using HuggingFaceBackend.Models.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace HuggingFaceBackend.Services.Impl
{
    /// <summary>
    /// Сервис для взаимодействия с API Hugging Face
    /// </summary>
    public class HuggingFaceApiClient : IHuggingFaceApiClient
    {
        private readonly IOptions<HuggingFaceAPISection> _apiOptions;
        private readonly ILogger<HuggingFaceApiClient> _logger;

        public HuggingFaceApiClient(
            IOptions<HuggingFaceAPISection> apiOptions,
            ILogger<HuggingFaceApiClient> logger)
        {
            this._apiOptions = apiOptions;
            this._logger = logger;
        }
        public async Task<IResponse?> GetResult(HuggingFaceAPIRequestDto request)
        {
            IResponse? result = null;
            var responseContent = string.Empty;

            try
            {
                responseContent = await this.ExecuteRequest(request);

                switch (request.TaskType)
                {
                    case TaskType.TextClassification:

                        result = new Response<List<HuggingFaceAPITextClassificationResponse>>
                        {
                            Data = JsonConvert.DeserializeObject<List<List<HuggingFaceAPITextClassificationResponse>>>(responseContent)
                                .FirstOrDefault()
                        };
                        break;

                    case TaskType.TextSummarization:

                        result = new Response<List<HuggingFaceAPITextSummarizeResponse>>
                        {
                            Data = JsonConvert.DeserializeObject<List<HuggingFaceAPITextSummarizeResponse>>(responseContent)
                        };
                        break;

                }
            }
            catch (Exception ex) 
            {
                this._logger.LogError(ex, ex.Message + $", received content: {responseContent}");

                try
                {
                    var errorResponse = JsonConvert.DeserializeObject<HuggingFaceAPIErrorResponse>(responseContent);
                    result = new ErrorResponse($"{errorResponse!.Error}, estimated time: {errorResponse.EstimatedTime}");
                }
                catch
                {
                    throw;
                }
            }

            return result;
        }
          
        /// <summary>
        /// Получить текстовый ответ при выполнении запроса
        /// </summary>
        /// <param name="request">ДТО-модель запроса</param>
        /// <returns>Строка ответа</returns>
        private async Task<string> ExecuteRequest(HuggingFaceAPIRequestDto request)
        {
            var apiUrl = this._apiOptions.Value.HuggingFaceModelsApiURL + request.ModelName;
            var apiKey = this._apiOptions.Value.HuggingFaceApiKey;

            var jsonRequestData = JsonConvert.SerializeObject(new HuggingFaceAPIRequest { Inputs = request.Inputs });
            var requestContent = new StringContent(
                jsonRequestData,
                Encoding.UTF8,
                "application/json");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme, 
                apiKey);
           
            var responseMsg = await client.PostAsync(apiUrl, requestContent);
            var responseContent = await responseMsg.Content.ReadAsStringAsync();

            return responseContent;
        }
    }
}
