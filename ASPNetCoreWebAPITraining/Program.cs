using ASPNetCoreWebAPITraining;
using ASPNetCoreWebAPITraining.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Register DbContext for EF Core
builder.Services.AddDbContext<MySqlDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString(StaticConfiguration.MySql),new MySqlServerVersion(StaticConfiguration.MySqlVersion))
);
builder.Services.AddDbContext<SqlServerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(StaticConfiguration.SqlServer))
);
builder.Services.AddDbContext<SQLite3DbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString(StaticConfiguration.SQLite3))
);
builder.Services.AddDbContext<MicrosoftAccessDbContext>(options =>
    options.UseJet(builder.Configuration.GetConnectionString(StaticConfiguration.MicrosoftAccess))
);

var provider = builder.Configuration.GetValue("Provider", string.Empty);

builder.Services.AddTransient<IDbContextFactory>(_ => provider switch
{
    StaticConfiguration.MySql => new MySqlDbContextFactory(builder.Configuration),
    StaticConfiguration.SqlServer => new SqlServerDbContextFactory(builder.Configuration),
    StaticConfiguration.SQLite3 => new SQLite3DbContextFactory(builder.Configuration),
    StaticConfiguration.MicrosoftAccess => new MicrosoftAccessDbContextFactory(builder.Configuration),
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