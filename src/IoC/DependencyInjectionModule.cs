using Application.Interfaces.Repositories;
using Application.Mappings;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IoC;

public static class DependencyInjectionModule
{
    public static void Register(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfiles));

        var assemblyRepositoryBase = Assembly.GetAssembly(typeof(RepositoryBase<>)); // Obtém o assembly que contém a classe RepositoryBase

        var repositoryInterfaceType = typeof(IRepositoryBase<>); // Obtém o tipo IRepositoryBase<>
        var repositoryBaseType = typeof(RepositoryBase<>); // Obtém o tipo RepositoryBase<>

        var assemblyInterfaces = Assembly.GetAssembly(typeof(IRepositoryBase<>)); // Obtém o assembly que contém as interfaces de repositório

        var repositoryInterfaces = assemblyInterfaces?.GetTypes()
            .Where(t => t.IsInterface && // Filtra apenas as interfaces
                        t.GetInterfaces().Any(i => i.IsGenericType && // Que são genéricas
                                                   i.GetGenericTypeDefinition() == repositoryInterfaceType)) // E que herdam de IRepositoryBase<>
            .ToList(); // Converte os tipos encontrados para uma lista

        var repositoryImplementations = assemblyRepositoryBase?.GetTypes()
             .Where(t => !t.IsAbstract && // Filtra as classes concretas
                         t.BaseType != null && // Que têm uma classe base
                         t.BaseType.IsGenericType && // Cuja classe base é genérica
                         t.BaseType.GetGenericTypeDefinition() == repositoryBaseType && // E a classe base é RepositoryBase<>
                         t.GetInterfaces().Any(i => i.IsGenericType && // E implementam uma interface genérica
                                                    i.GetGenericTypeDefinition() == repositoryInterfaceType && // Que é IRepositoryBase<>
                                                    i != repositoryInterfaceType)) // E não é a própria IRepositoryBase<>
             .ToList() ?? []; // Converte as implementações de repositório encontradas para uma lista

        foreach (var implementation in repositoryImplementations)
        {
            var interfaceName = "I" + implementation.Name; // Constrói o nome da interface correspondente adicionando um "I" antes do nome da classe de implementação

            var interfaceType = repositoryInterfaces?.FirstOrDefault(i => i.Name == interfaceName); // Encontra a interface correspondente pelo nome

            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, implementation); // Registra a implementação de repositório com sua interface correspondente no contêiner de injeção de dependência
            }
        }
    }

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(e => e.UseNpgsql(configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
    }

    public static void Migrate(IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            dbContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro durante a migração do banco de dados: {ex.Message}");
        }
    }
}
