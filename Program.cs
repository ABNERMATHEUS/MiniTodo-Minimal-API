using MiniTodo.Endpoints;
using MiniTodo.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddSwagger()
    .AddMongoDb(builder.Configuration)
    .AddFluentValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapTodoEndpoints();

app.Run();