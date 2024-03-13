using FluentValidation;
using MiniTodo.Collections;
using MiniTodo.Endpoints.Validators;
using MongoDB.Driver;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace MiniTodo.Extensions;

public static class ConfigurationsExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;

    }
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("MongoDb")!;
        var mongoClient = new MongoClient(connectionString);
        var database = mongoClient.GetDatabase("DatabaseTodo");
        services.AddSingleton(mongoClient);
        services.AddSingleton(database);
        services.AddSingleton(x=> database.GetCollection<Todo>("Todos"));
        return services;
    }

    public static void AddFluentValidation(this IServiceCollection services)
    {        
        services.AddValidatorsFromAssemblyContaining<CreateTodoRequestValidator>();
        services.AddFluentValidationAutoValidation();
    }
}