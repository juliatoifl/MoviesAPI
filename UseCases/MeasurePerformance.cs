namespace MoviesAPI.UseCases;

using System.Diagnostics;
using Dapper;
using Microsoft.Data.SqlClient;

public class MeasurePerformance
{
    private readonly string _connectionString;

    public MeasurePerformance(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<long> Insert()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                for (int i = 0; i < 10000; i++)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO Actor (FirstName, LastName, DateOfBirth) VALUES (@FirstName, @LastName, @DateOfBirth)",
                        new { FirstName = $"Actor{i}", LastName = "Test", DateOfBirth = DateTime.UtcNow },
                        transaction);
                }

                transaction.Commit();
                stopwatch.Stop();
                return stopwatch.ElapsedMilliseconds;
            }
        }
    }
}
