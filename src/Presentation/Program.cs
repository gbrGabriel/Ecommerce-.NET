using Infrastructure.Context;
using Infrastructure.SeedData;
using IoC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Presentation.Errors;
using Presentation.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Ecommerce API",
            Description = "APIs for Ecommerce project",
            Version = "2024.04.1.0",
            Contact = new OpenApiContact
            {
                Email = "gabrielgbr.contato@gmail.com",
                Name = "Gabriel Silva",
                Url = new Uri("https://www.linkedin.com/in/gbrgabriel/")
            },
            License = new OpenApiLicense
            {
                Name = "WTFPL",
                Url = new Uri("https://en.wikipedia.org/wiki/WTFPL")
            }
        }
    );

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext(builder.Configuration);

builder.Services.Register();

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.InvalidModelStateResponseFactory = actionContext =>
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToArray();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        var errorResponse = new ValidationErrorResponseApi
        {
            Errors = errors
        };
        return new BadRequestObjectResult(errorResponse);
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceScopeFactory = scope.ServiceProvider.GetRequiredService<IServiceScopeFactory>();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    DependencyInjectionModule.Migrate(serviceScopeFactory);
    await SeedDataApplicationContext.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();


