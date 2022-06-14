﻿using Microsoft.IdentityModel.Tokens;
using MoneyFamily.WebApi.AuthTest.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MoneyFamily.WebApi.AuthTest.JwtHelpers
{
    public static class JwtHelpers
    {
        public static IEnumerable<Claim> GetClaims(Guid Id)
        {
            IEnumerable<Claim> claims = new Claim[] {
                new Claim(JwtRegisteredClaimNames.Sub, Id.ToString()),
                //new Claim("Id", userAccounts.Id.ToString()),
                //    new Claim(ClaimTypes.Name, userAccounts.UserName),
                //    new Claim(ClaimTypes.Email, userAccounts.EmailId),
                //    new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                //    new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString("MMM ddd dd yyyy HH:mm:ss tt"))
            };
            return claims;
        }
        public static IEnumerable<Claim> GetClaims(out Guid Id)
        {
            Id = Guid.NewGuid();
            //return GetClaims(userAccounts, Id);
            return GetClaims(Id);
        }
        public static UserTokens GenTokenkey(JwtSettings jwtSettings)
        {
            try
            {
                var UserToken = new UserTokens();
                //if (model == null) throw new ArgumentException(nameof(model));
                // Get secret key
                var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
                Guid Id = Guid.Empty;
                DateTime expireTime = DateTime.UtcNow.AddDays(1);
                //UserToken.Validaty = expireTime.TimeOfDay;
                var JWToken = new JwtSecurityToken(issuer: jwtSettings.ValidIssuer, audience: jwtSettings.ValidAudience, claims: GetClaims(out Id), notBefore: new DateTimeOffset(DateTime.Now).DateTime, expires: new DateTimeOffset(expireTime).DateTime, signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));
                UserToken.Token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                //UserToken.UserName = model.UserName;
                //UserToken.Id = model.Id;
                //UserToken.GuidId = Id;
                return UserToken;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
