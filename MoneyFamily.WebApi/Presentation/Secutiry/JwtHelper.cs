using Microsoft.IdentityModel.Tokens;
using MoneyFamily.WebApi.Domain.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoneyFamily.WebApi.Presentation.Secutiry
{
    public class JwtHelper
    {
        private static IEnumerable<Claim> GetClaims(Guid id)
        {
            var now = new DateTimeOffset(DateTime.UtcNow);
            return new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
        }

        public static string GenToken(Guid id, JwtSetting jwtSettings)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
                DateTime nowTime = DateTime.UtcNow;
                DateTime expireTime = DateTime.UtcNow.AddDays(7);
                var jwt = new JwtSecurityToken(
                    issuer: jwtSettings.ValidIssuer,
                    audience: jwtSettings.ValidAudience,
                    claims: GetClaims(id),
                    notBefore: nowTime,
                    expires: expireTime,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));
                var token = new JwtSecurityTokenHandler().WriteToken(jwt);
                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
