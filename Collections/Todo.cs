using MiniTodo.Endpoints.Dtos;
using MiniTodo.Endpoints.Validators;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniTodo.Collections;


public class Todo
{
    public Todo(string title, string description)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        IsDone = false;
    }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public bool IsDone { get; private set; }

    public void Update(UpdateTodoRequest request)
    {        
        Title = request.Title;
        Description = request.Description;
        IsDone = request.IsDone;
    }

    public static explicit operator Todo(CreateTodoRequest request)
    {
        return new Todo(request.Title, request.Description);
    }
    
    public static explicit operator TodoDto(Todo todo)
    {
        return new TodoDto(todo.Id, todo.Title, todo.Description, todo.IsDone);
    }
}
