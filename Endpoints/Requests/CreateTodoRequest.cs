using FluentValidation;
namespace MiniTodo.Endpoints.Validators;
public record CreateTodoRequest(string Title, string Description);
public class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}