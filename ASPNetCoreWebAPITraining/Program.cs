using ASPNetCoreWebAPITraining.Database;
using ASPNetCoreWebAPITraining.Documentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersonDatabase(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddPersonApiDocumentation();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapPersonApiDocumentation();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
