using AddressParser.Application.Common.Models;
using AddressParser.Application.DTOs;
using AddressParser.Domain.Entities;

namespace AddressParser.Application.Interfaces;

public interface IAddressRepository
{
    Task AddAsync(ParsedAddress address);
    Task<IEnumerable<ParsedAddress>> GetAllAsync();
    Task<ParsedAddress?> GetByIdAsync(Guid id);
    Task UpdateAsync(ParsedAddress address);
    Task DeleteAsync(Guid id);
    Task<PagedResult<ParsedAddress>> SearchPagedAsync(AddressFilter filter);
    Task<IEnumerable<ParsedAddress>> GetAllByFilterAsync(AddressFilter filter);

}