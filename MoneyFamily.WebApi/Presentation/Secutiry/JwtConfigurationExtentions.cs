using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MoneyFamily.WebApi.AuthTest.Models;

namespace MoneyFamily.WebApi.Presentation.Secutiry
{
    public static class JwtConfigurationExtentions
    {
        public static void AddJwtTokenServices(IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = new JwtSettings();
            //appsetttting.jsonのJwtConfigをバインドする
            //configuration.Bind("JwtConfig", jwtConfig);
            configuration.GetSection("JsonWebTokenKeys").Bind(jwtConfig);
            services.AddSingleton(jwtConfig);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = jwtConfig.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtConfig.IssuerSigningKey)),
                    ValidateIssuer = jwtConfig.ValidateIssuer,
                    ValidIssuer = jwtConfig.ValidIssuer,
                    ValidateAudience = jwtConfig.ValidateAudience,
                    ValidAudience = jwtConfig.ValidAudience,
                    RequireExpirationTime = jwtConfig.RequireExpirationTime,
                    ValidateLifetime = jwtConfig.ValidateLifetime,
                    ClockSkew = TimeSpan.Zero,
                };
            });
        }

    }
}
