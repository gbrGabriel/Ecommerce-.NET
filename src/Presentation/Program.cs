using Infrastructure.Context;
using Infrastructure.SeedData;
using IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.Register();

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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();


