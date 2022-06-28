using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MoneyFamily.WebApi.Presentation.Secutiry
{
    public static class JwtConfigurationExtension
    {
        public static void AddJwtServices(WebApplicationBuilder builder)
        {
            var bindJwtSettings = new JwtSetting();
            builder.Configuration.Bind("JwtSettings", bindJwtSettings);
            builder.Services.AddSingleton(bindJwtSettings);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey)),
                    ValidateIssuer = bindJwtSettings.ValidateIssuer,
                    ValidIssuer = bindJwtSettings.ValidIssuer,
                    ValidateAudience = bindJwtSettings.ValidateAudience,
                    ValidAudience = bindJwtSettings.ValidAudience,
                    RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
                    ValidateLifetime = bindJwtSettings.ValidateLifetime,
                    ClockSkew = TimeSpan.Zero,
                };
            });
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
