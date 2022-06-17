using Microsoft.EntityFrameworkCore;
using MoneyFamily.WebApi;
using MoneyFamily.WebApi.Infrastructure;
using MoneyFamily.WebApi.Presentation.Secutiry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
JwtConfigurationExtensions.AddJwtServices(builder);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(new string[] { "http://localhost:4200" })
    );

    options.AddPolicy("CorsPolicyName",
         builder => builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(new string[] { "http://localhost:8080" })
        );
});

//DI
DependencyInjectionExtenstions.AddDependencyInjection(builder);


//DbContext
builder.Services.AddDbContext<MoneyFamilyContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("MoneyFamilyContext"));
});
//builder.Services.AddMvc().AddJsonOptions(options =>
//{
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
//});

//solt


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();
