using ASPNetCoreWebAPITraining.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MySqlDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySql"), new MySqlServerVersion(new Version(8, 3, 0)))
);
builder.Services.AddDbContext<SqlServerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
);


var provider = builder.Configuration.GetValue("Provider", "Unsupported");

builder.Services.AddTransient<IDbContextFactory>(_ => provider switch
{
    "MySql" => new MySqlDbContextFactory(builder.Configuration),
    "SqlServer" => new SqlServerDbContextFactory(builder.Configuration),
    _ => throw new Exception($"Unsupported provider: {provider}")
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Person API", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Person API v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();