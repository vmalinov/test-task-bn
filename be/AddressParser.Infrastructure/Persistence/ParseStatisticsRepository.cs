using AddressParser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class ParseStatisticsRepository : IParseStatisticsRepository
{
    private readonly AddressDbContext _context;

    public ParseStatisticsRepository(AddressDbContext context)
    {
        _context = context;
    }

    public async Task<ParseStatistics?> GetAsync()
    {
        return await _context.ParseStatistics.FirstOrDefaultAsync();
    }

    public async Task AddAsync(ParseStatistics stats)
    {
        _context.ParseStatistics.Add(stats);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ParseStatistics stats)
    {
        _context.ParseStatistics.Update(stats);
        await _context.SaveChangesAsync();
    }
}
