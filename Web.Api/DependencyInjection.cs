using System.Text.Json;
using System.Text.Json.Serialization;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGenWithAuth();
        services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                        options.JsonSerializerOptions.TypeInfoResolver = new PolymorphicTypeResolver();
                    });


        services.AddCors(options =>
        {
            options.AddPolicy("Development", policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
        });
        });

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
