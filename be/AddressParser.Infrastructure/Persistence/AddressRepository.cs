using AddressParser.Application.Common.Models;
using AddressParser.Application.DTOs;
using AddressParser.Application.Interfaces;
using AddressParser.Domain.Entities;
using AddressParser.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AddressParser.Infrastructure.Persistence;

public class AddressRepository : IAddressRepository
{
    private readonly AddressDbContext _context;

    public AddressRepository(AddressDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ParsedAddress address)
    {
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ParsedAddress>> GetAllAsync()
    {
        return await _context.Addresses.AsNoTracking().OrderBy(x=>x.Id).ToListAsync();
    }
    public async Task<ParsedAddress?> GetByIdAsync(Guid id)
    {
        return await _context.Addresses.FindAsync(id);
    }

    public async Task UpdateAsync(ParsedAddress address)
    {
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Addresses.FindAsync(id);
        if (entity is not null)
        {
            _context.Addresses.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<ParsedAddress>> GetAllByFilterAsync(AddressFilter filter)
    {
        var query = _context.Addresses
            .AsQueryable()
            .ApplyFilter(filter);

        return await query.ToListAsync();
    }

    public async Task<PagedResult<ParsedAddress>> SearchPagedAsync(AddressFilter filter)
    {
        var query = _context.Addresses
            .AsQueryable()
            .ApplyFilter(filter);

        var totalCount = await query.CountAsync();

        var items = await query.OrderBy(x=>x.Id)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<ParsedAddress>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize
        };
    }
}
