using HuggingFaceBackend.Helpers;
using HuggingFaceBackend.Models.ConfigSections;
using HuggingFaceBackend.Models.DTO;
using HuggingFaceBackend.Models.DTO.Authorization;
using HuggingFaceBackend.Models.DTO.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HuggingFaceBackend.Controllers
{
    /// <summary>
    /// Контроллер для регистрации-авторизации
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<DatabaseSection> _dbOptions;
        private readonly string databaseDirectoryPath;
        private readonly string databaseFilePath;

        public AuthController(IOptions<DatabaseSection> dbOptions) 
        {
            this._dbOptions = dbOptions;

            this.databaseDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), this._dbOptions.Value.DataBaseFolderName);
            this.databaseFilePath = Path.Combine(this.databaseDirectoryPath, this._dbOptions.Value.DataBaseTextFileName);
        }

        /// <summary>
        /// Эндпоинт регистрации
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("registrate")]
        public async Task<IActionResult> Registrate(RegistrationRequestDto request)
        {
            try
            {
                this.EnsureDatabaseFileExists();

                if (request.HashedPassword != request.HashedCheckPassword)
                {
                    throw new Exception("Passwords are not the same");
                }

                var usersJsonText = await System.IO.File.ReadAllTextAsync(databaseFilePath);
                var usersList = JsonConvert.DeserializeObject<List<User>>(usersJsonText) ?? new List<User>();

                if (usersList is not null && usersList.Any(u => u.Username.Equals(request.Username)))
                {
                    throw new Exception("User with that username already exists");
                }

                usersList!.Add(
                    new User
                    {
                        Username = request.Username,
                        HashedPassword = request.HashedPassword,
                        Id = Guid.NewGuid()
                    });

                usersJsonText = JsonConvert.SerializeObject(usersList);

                await System.IO.File.WriteAllTextAsync(databaseFilePath, usersJsonText);

                return Ok("User is created successfully");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Эндпоинт авторизации
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("authorize")]
        public async Task<IActionResult> Authorize(AuthorizationRequestDto request)
        {
            try
            {
                this.EnsureDatabaseFileExists();

                var usersJsonText = await System.IO.File.ReadAllTextAsync(databaseFilePath);
                var usersList = JsonConvert.DeserializeObject<List<User>>(usersJsonText) ?? throw new Exception("Invalid login or password");

                var user = usersList.FirstOrDefault(u => u.Username.Equals(request.Username));

                if (user is null) 
                {
                    throw new Exception("Invalid login or password");
                }
                else if (!user.HashedPassword.Equals(request.HashedPassword))
                {
                    throw new Exception("Invalid login or password");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                };
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(120)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
                var jwtString = new JwtSecurityTokenHandler().WriteToken(jwt);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(120)
                };

                HttpContext.Response.Cookies.Append("token", jwtString, cookieOptions);
                return Ok("Authorization success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Эндпоинт выхода
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMonths(-1)
            };

            HttpContext.Response.Cookies.Delete("token", cookieOptions);
            return Ok("Logged out successfully");
        }

        /// <summary>
        /// Убедиться, что файл базы данных создан
        /// </summary>
        private void EnsureDatabaseFileExists()
        {
            if (!Directory.Exists(databaseDirectoryPath))
            {
                Directory.CreateDirectory(databaseDirectoryPath);
            }

            var dbFileInfo = new FileInfo(databaseFilePath);
            if (!dbFileInfo.Exists)
            {
                dbFileInfo.Create();
            }
        }
    }
}
