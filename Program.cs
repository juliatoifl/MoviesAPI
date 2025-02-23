using MoviesAPI.Endpoints;
using MoviesAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Services zone - BEGIN

builder.Services.AddScoped<IGenresRepository, GenresRepository>();
builder.Services.AddScoped<IActorsRepository, ActorsRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

// Services zone - END

var app = builder.Build();

// Middleware zone - BEGIN

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello world!");

app.MapGroup("/genres").MapGenres();
app.MapGroup("/actors").MapActors();

// Middleware zone - END
app.Run();