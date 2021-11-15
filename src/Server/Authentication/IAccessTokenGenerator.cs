using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace Nova.Identity.Authentication
{
    public interface IAccessTokenGenerator
    {
        AccessToken Generate(TokenGenerationPayload payload);
    }

    sealed class AccessTokenGenerator : IAccessTokenGenerator
    {
        static readonly JwtSecurityTokenHandler _tokenHandler = new();

        public AccessToken Generate(TokenGenerationPayload payload)
        {
            throw new NotImplementedException();
        }
    }
}