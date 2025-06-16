

namespace AddressParser.Application.Services;
public class ParseStatsService : IParseStatsService
{
    private readonly IParseStatisticsRepository _repository;

    public ParseStatsService(IParseStatisticsRepository repository)
    {
        _repository = repository;
    }


    //NOTE: можно добавить сохранение каждого парса и его время выполнения как отдельную запись, чтобы потом отображать в графике
    public async Task<ParseStatistics> GetStatsAsync()
    {
        return await _repository.GetAsync() ?? new ParseStatistics();
    }

    public async Task UpdateSuccessAsync(double durationMs)
    {
        var stats = await _repository.GetAsync();
        if (stats == null)
        {
            stats = new ParseStatistics
            {
                TotalParsed = 1,
                LastSuccess = DateTime.UtcNow,
                AvgParseDurationMs = durationMs
            };
            await _repository.AddAsync(stats);
        }
        else
        {
            stats.TotalParsed++;
            stats.LastSuccess = DateTime.UtcNow;
            stats.AvgParseDurationMs = stats.AvgParseDurationMs.HasValue
                ? (stats.AvgParseDurationMs + durationMs) / 2
                : durationMs;

            await _repository.UpdateAsync(stats);
        }
    }

    public async Task UpdateFailureAsync()
    {
        var stats = await _repository.GetAsync();
        if (stats == null)
        {
            stats = new ParseStatistics
            {
                TotalFailed = 1,
                LastFailure = DateTime.UtcNow
            };
            await _repository.AddAsync(stats);
        }
        else
        {
            stats.TotalFailed++;
            stats.LastFailure = DateTime.UtcNow;
            await _repository.UpdateAsync(stats);
        }
    }
}
