public interface IParseStatsService
{
    Task UpdateSuccessAsync(double durationMs);
    Task UpdateFailureAsync();
    Task<ParseStatistics> GetStatsAsync();
}
