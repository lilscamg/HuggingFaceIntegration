using HuggingFaceBackend.Models.DTO.HuggingFaceAPI;
using HuggingFaceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HuggingFaceBackend.Controllers
{
    [ApiController]
    [Route("api")]
    public class HuggingFaceIntegrationController : ControllerBase
    {
        private readonly IHuggingFaceApiClient _huggingFaceApiClient;

        public HuggingFaceIntegrationController(IHuggingFaceApiClient huggingFaceApiClient)
        {
            this._huggingFaceApiClient = huggingFaceApiClient;
        }

        [HttpPost]
        [Authorize]
        [Route("get-result")]
        public async Task<IActionResult> GetResult(HuggingFaceAPIRequestDto request)
        {
            try
            {
                var result = await this._huggingFaceApiClient.GetResult(request);
                return result!.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex);
            }
        }
    }
}
