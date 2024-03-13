using FluentValidation;
namespace MiniTodo.Endpoints.Validators;
public record UpdateTodoRequest(Guid Id, string Title, string Description, bool IsDone);
public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
{
   public UpdateTodoRequestValidator()
    {
       RuleFor(x => x.Id).NotEmpty();
       RuleFor(x => x.Title).NotEmpty();
       RuleFor(x => x.Description).NotEmpty();       
   }
}