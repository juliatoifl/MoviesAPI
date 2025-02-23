using MoviesAPI.Endpoints;
using MoviesAPI.Genres;

var builder = WebApplication.CreateBuilder(args);

// Services zone - BEGIN

builder.Services.AddScoped<IGenresRepository, GenresRepository>();

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

// Middleware zone - END
app.Run();