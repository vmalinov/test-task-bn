using System.Text;
using AddressParser.Application.DTOs;
using AddressParser.Application.Interfaces;
using AddressParser.Application.Services;
using AddressParser.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AddressParser.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;
    private readonly IParseStatsService _parseStatsService;

    public AddressController(IAddressService addressService, IParseStatsService parseStatsService)
    {
        _addressService = addressService;
        _parseStatsService = parseStatsService;
    }


    [HttpPost("parse")]
    public async Task<IActionResult> Parse([FromBody] ParseRequest request)
    {
        try
        {
            var parsed = await _addressService.ParseAndSaveAsync(request.RawAddress);
            return Ok(parsed);
        }
        catch (FormatException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var addresses = await _addressService.GetAllAsync();
        return Ok(addresses);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAddress dto)
    {
        var created = await _addressService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _addressService.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ParsedAddress updated)
    {
        if (id != updated.Id)
            return BadRequest("ID mismatch");

        await _addressService.UpdateAsync(updated);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _addressService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] AddressFilter filter)
    {
        var result = await _addressService.SearchPagedAsync(filter);
        return Ok(result);
    }

    [HttpGet("export/json")]
    public async Task<IActionResult> ExportJson([FromQuery] AddressFilter filter)
    {
        var json = await _addressService.ExportToJsonAsync(filter);
        return File(Encoding.UTF8.GetBytes(json), "application/json", "addresses.json");
    }

    [HttpGet("export/csv")]
    public async Task<IActionResult> ExportCsv([FromQuery] AddressFilter filter)
    {
        var csvBytes = await _addressService.ExportToCsvAsync(filter);
        return File(csvBytes, "text/csv", "addresses.csv");
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = await _parseStatsService.GetStatsAsync();
        return Ok(stats);
    }

    [HttpGet("generate")]
    public async Task<IActionResult> Generate()
    {
        for (int i = 0; i < 100; i++)
        {
            await _addressService.ParseAndSaveAsync(AddressGenerator.GenerateRandomAddress());
        }
        return Ok();
    }

}
