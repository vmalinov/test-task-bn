using AddressParser.Application.Common.Models;
using AddressParser.Application.DTOs;
using AddressParser.Domain.Entities;

namespace AddressParser.Application.Interfaces;

public interface IAddressService
{
    Task<ParsedAddress> ParseAndSaveAsync(string rawAddress);
    Task<IEnumerable<ParsedAddress>> GetAllAsync();
    Task<ParsedAddress?> GetByIdAsync(Guid id);
    Task UpdateAsync(ParsedAddress address);
    Task DeleteAsync(Guid id);
    Task<ParsedAddress> CreateAsync(CreateAddress dto);
    Task<PagedResult<ParsedAddress>> SearchPagedAsync(AddressFilter filter);
    Task<string> ExportToJsonAsync(AddressFilter filter);
    Task<byte[]> ExportToCsvAsync(AddressFilter filter);

}
