using Infrastructure.Context;
using Infrastructure.SeedData;
using IoC;
using Microsoft.AspNetCore.Mvc;
using Presentation.Errors;
using Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.Register();

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToArray();

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


