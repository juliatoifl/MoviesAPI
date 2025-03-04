using MoviesAPI.UseCases;

namespace MoviesAPI.Endpoints;

public static class PerformanceEndpoints
{
    public static RouteGroupBuilder MapPerformance(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (MeasurePerformance measurePerformance) =>
        {
            long elapsedTime = await measurePerformance.Insert();
            return Results.Ok(new { ElapsedMilliseconds = elapsedTime });
        });

        return group;
    }
}