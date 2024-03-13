using MiniTodo.Collections;
using MiniTodo.Endpoints.Dtos;
using MiniTodo.Endpoints.Validators;
using MongoDB.Bson;
using MongoDB.Driver;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace MiniTodo.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this WebApplication app)
    {
        #region GET
        
        app.MapGet("/todos", async (IMongoCollection<Todo> collection, CancellationToken cancellationToken, int page = 1, int pageSize = 10) =>
        {
            var todos = await collection.Find(new BsonDocument()).Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync(cancellationToken);
            var todosDto = todos.Select(x => (TodoDto)x);
            return Results.Ok(todosDto);
        });

        app.MapGet("/todos/{id}", async (IMongoCollection<Todo> collection, Guid id, CancellationToken cancellationToken) =>
        {
            var todo = await collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
            if (todo is null)
            {
                return Results.NotFound();
            }
            var todoDto = (TodoDto)todo;
            return Results.Ok(todoDto);
        });
        #endregion

        #region POST

        app.MapPost("/todos", async (IMongoCollection<Todo> collection, CreateTodoRequest request, CancellationToken cancellationToken) =>
        {
            var todo = (Todo)request;
            await collection.InsertOneAsync(todo, cancellationToken: cancellationToken);
            return Results.Created($"/todos/{todo.Id}", todo);
        }).AddFluentValidationAutoValidation();

        #endregion

        #region PUT
        app.MapPut("/todos/{id}", async (IMongoCollection<Todo> collection, UpdateTodoRequest request, CancellationToken cancellationToken) =>
        {
            var todo = await collection.Find(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (todo is null)
            {
                return Results.NotFound();
            }

            todo.Update(request);
            await collection.ReplaceOneAsync(x => x.Id == request.Id, todo, cancellationToken: cancellationToken);
            return Results.NoContent();
        }).AddFluentValidationAutoValidation();
        #endregion

        #region DELETE
        app.MapDelete("/todos/{id}", async (IMongoCollection<Todo> collection, Guid id, CancellationToken cancellationToken) =>
            {
                var result = await collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
                if (result.DeletedCount == 0)
                {
                    return Results.NotFound();
                }
                return Results.NoContent();
            });
        #endregion
    }
}
