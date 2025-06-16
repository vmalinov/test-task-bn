public interface IParseStatisticsRepository
{
    Task<ParseStatistics?> GetAsync();
    Task UpdateAsync(ParseStatistics stats);
    Task AddAsync(ParseStatistics stats);
}
