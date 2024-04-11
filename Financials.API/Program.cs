
using Financials.Core.Entity;
using Financials.Infrastructure;
using Financials.Infrastructure.Configuraton;
using Financials.Infrastructure.Context;
using Financials.Infrastructure.Repositorio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Financials.Services;
using Financials.Infrastructure.Seeds;
using Financials.API.Middlewares;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Financials.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddSwaggerGen(options =>
            {
                var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo()
                    {
                        Title = $"Financials {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = "Api principal da aplicação de gerenciamento financeiro Financials"
                    });
                }
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Autorização via JWT no header usando bearer.Example: \"Authorization: Bearer {token}\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<FinancialsDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<JWTConfiguration>(opt => builder.Configuration.GetSection("Jwt").Bind(opt));

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddInitialSeedData();
            builder.Services.AddValidators();
            builder.Services.AddServices();

            builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .MinimumLevel.Information() // Define o nível mínimo global
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Suprime a maioria dos logs de frameworks/componentes externos
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("MinhaAplicacao.API", LogEventLevel.Information) // Ajusta para o seu namespace
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "gow-academy-logs-{0:yyyy.MM}"
            }));

            var app = builder.Build();

            app.UseMiddleware<LoggingMiddleware>();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        $"Financials {description.GroupName.ToUpperInvariant()}"
                    );
                }
                options.RoutePrefix = string.Empty;
            });

            SeedUserRoles(app);

            app.UseAuthorization();


            app.MapControllers();
            

            app.Run();

            void SeedUserRoles(IApplicationBuilder app)
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var seed = serviceScope.ServiceProvider.GetService<ISeedInitialUserAndRoles>();
                    seed.SeedRoles();
                    seed.SeedUsers();

                }
            }
        }
    }
}
