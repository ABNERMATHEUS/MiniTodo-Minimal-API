namespace MiniTodo.Endpoints.Dtos;
public record TodoDto(Guid Id, string Title, string Description, bool IsDone);