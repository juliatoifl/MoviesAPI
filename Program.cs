using MoviesAPI.Endpoints;
using MoviesAPI.Repositories;
using MoviesAPI.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Services zone - BEGIN

builder.Services.AddScoped<IGenresRepository, GenresRepository>();
builder.Services.AddScoped<IActorsRepository, ActorsRepository>();
builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
builder.Services.AddScoped<IActorMovieRepository, ActorMovieRepository>();
builder.Services.AddScoped<IAcademyAwardNominationRepository, AcademyAwardNominationRepository>();
builder.Services.AddScoped<IScandalLogRepository, ScandalLogRepository>();

builder.Services.AddScoped<CreateAcademyAwardNomination>();
builder.Services.AddScoped<ReactToScandal>();
builder.Services.AddScoped<BulkImportPastWinners>();
builder.Services.AddScoped<MeasurePerformance>();

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

app.MapGroup("/genres").MapGenres();
app.MapGroup("/actors").MapActors();
app.MapGroup("/movies").MapMovies();
app.MapGroup("/academy-awards").MapAcademyAwards();
app.MapGroup("/performance").MapPerformance();

// Middleware zone - END
app.Run();