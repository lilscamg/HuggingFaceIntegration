using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HuggingFaceBackend.Helpers
{
    public static class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "powerfulpaswword1234567890bulatchokolad";   // ключ для шифрации

        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
