
using Web.Api.Infrastructure;
using Web.Api.Extensions;
using Serilog;
using Application;
using Infrastructure;
using Web.Api;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;



            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));


            builder.Services
            .AddApplication()
            .AddPresentation()
            .AddInfrastructure(builder.Configuration);


            var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseCors("Development");
                    app.UseSwaggerWithUi();
                    app.ApplyMigrations();
                }
                else
                {
                    app.UseCors("Development");
                    app.UseSwaggerWithUi();
                    app.ApplyMigrations();
                }

app.MapHealthChecks("health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
       

            app.UseRequestContextLogging();

            app.UseSerilogRequestLogging();

            app.UseExceptionHandler();

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseHttpsRedirection();


            app.MapControllers();

            await app.RunAsync();



            //dotnet ef migrations add modelupdate --project Infrastructure --startup-project Web.Api

namespace Web.Api
    {
        public partial class Program;
    }

