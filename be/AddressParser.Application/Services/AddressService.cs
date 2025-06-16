using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using AddressParser.Application.Common.Models;
using AddressParser.Application.DTOs;
using AddressParser.Application.Interfaces;
using AddressParser.Domain.Entities;
using AddressParser.Domain.Errors;

namespace AddressParser.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _repository;
    private readonly IAddressParser _parser;
    private readonly IParseStatsService _parseStatsService;

    public AddressService(IAddressRepository repository, IAddressParser parser, IParseStatsService parseStatsService)
    {
        _repository = repository;
        _parser = parser;
        _parseStatsService = parseStatsService;
    }

    public async Task<ParsedAddress> ParseAndSaveAsync(string rawAddress){
        var sw = Stopwatch.StartNew();
        try
        {
            var parsed = _parser.Parse(rawAddress);
            sw.Stop();
            await _repository.AddAsync(parsed);
            await _parseStatsService.UpdateSuccessAsync(sw.Elapsed.TotalMilliseconds);
            return parsed;
        }
        catch
        {
            sw.Stop();
            await _parseStatsService.UpdateFailureAsync();
            throw;
        }
    }

    public Task<PagedResult<ParsedAddress>> SearchPagedAsync(AddressFilter filter)
    {
        return _repository.SearchPagedAsync(filter);
    }

    public Task<IEnumerable<ParsedAddress>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<ParsedAddress?> GetByIdAsync(Guid id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task UpdateAsync(ParsedAddress address)
    {
        var oldRecord = _repository.GetByIdAsync(address.Id).Result;
        if(oldRecord==null)
            throw new Exception(ErrorMessages.RecordNotFound);
        if (address.Version != oldRecord.Version) 
            throw new Exception(ErrorMessages.AlreadyUpdated);
        address.Version = Guid.NewGuid();
        oldRecord.Version =  Guid.NewGuid();
        oldRecord.RawInput = _parser.CreateRawAddress(address);
        oldRecord.City = address.City;
        oldRecord.Country = address.Country;
        oldRecord.HouseNumber = address.HouseNumber;
        oldRecord.OfficeOrRoom = address.OfficeOrRoom;
        oldRecord.Region = address.Region;
        oldRecord.Street = address.Street;
        return _repository.UpdateAsync(oldRecord);
    }

    public Task DeleteAsync(Guid id)
    {
        return _repository.DeleteAsync(id);
    }

    public async Task<ParsedAddress> CreateAsync(CreateAddress dto)
    {
        var address = new ParsedAddress
        {
            Id = Guid.NewGuid(),
            Country = dto.Country,
            Region = dto.Region,
            City = dto.City,
            Street = dto.Street,
            HouseNumber = dto.HouseNumber,
            OfficeOrRoom = dto.OfficeOrRoom,
            Version = Guid.NewGuid()
        };

        address.RawInput = _parser.CreateRawAddress(address);
        await _repository.AddAsync(address);
        return address;
    }

    //NOTE: в экспорте возможна проблема с большим объемом данных если мы выбираем без фильтров или при фильтрах с большой выборкой,
    // можно рассчитать количество данных и сделать выгрузку по частям

    public async Task<string> ExportToJsonAsync(AddressFilter filter)
    {
        var data = await _repository.GetAllByFilterAsync(filter);
        return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
    }

    public async Task<byte[]> ExportToCsvAsync(AddressFilter filter)
    {
        var data = await _repository.GetAllByFilterAsync(filter);
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        using var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteHeader<ParsedAddress>();
        await csv.NextRecordAsync();
        await csv.WriteRecordsAsync(data);
        await writer.FlushAsync();

        return memoryStream.ToArray();
    }


}
