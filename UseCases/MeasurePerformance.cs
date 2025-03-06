using System.Text;

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

                int batchSize = 50;
                for (int i = 0; i < 10000; i += batchSize)
                {
                    var sql = new StringBuilder("INSERT INTO Actor (FirstName, LastName, DateOfBirth) VALUES ");
                    var parameters = new DynamicParameters();
                    List<string> valueRows = new List<string>();

                    for (int j = 0; j < batchSize && (i + j) < 10000; j++)
                    {
                        valueRows.Add($"(@FirstName{i+j}, @LastName{i+j}, @DateOfBirth{i+j})");
                        parameters.Add($"FirstName{i+j}", $"Actor{i+j}");
                        parameters.Add($"LastName{i+j}", "Test");
                        parameters.Add($"DateOfBirth{i+j}", DateTime.UtcNow);
                    }

                    sql.Append(string.Join(",", valueRows));
                    await connection.ExecuteAsync(sql.ToString(), parameters, transaction);
                }

                transaction.Commit();
                stopwatch.Stop();
                return stopwatch.ElapsedMilliseconds;
            }
        }
    }
}
