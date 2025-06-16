using System.Text.RegularExpressions;
using AddressParser.Application.DTOs;
using AddressParser.Application.Interfaces;
using AddressParser.Domain.Entities;
using AddressParser.Domain.Errors;

namespace AddressParser.Infrastructure.Parsers;

public class SimpleAddressParser : IAddressParser
{
    private static readonly Regex countryPattern = new Regex(@"\p{L}+(\s\p{L}+)*");
    private static readonly Regex regionPattern = new Regex(@"\p{L}+\s+(обл\.|область)(\.|)(.*)?");
    private static readonly Regex cityPattern = new Regex(@"(г\.|город)\s*\p{L}+");
    private static readonly Regex streetPattern = new Regex(@"(ул\.|улица)\s*\p{L}+");
    private static readonly Regex housePattern = new Regex(@"\b\d+(\s*-\s*\d+)|((дом|д\.\s*)?\d+)\b");
    private static readonly Regex houseAndApartmentPattern = new Regex(@"\b\d+(\s*-\s*\d+)\b");
    private static readonly Regex additionalInfoPattern = new Regex(@"\b\d+|(квартира|кабинет|кв.)?\s*\d+\b");

    public string CreateRawAddress(ParsedAddress address)
    {
        return $"{address.Country}, {address.Region}, {address.City}, {address.Street}, {address.HouseNumber}, {address.OfficeOrRoom}";
    }

    public ParsedAddress Parse(string input)
    {
        if (!input.Contains(','))
            throw new FormatException($"{ErrorMessages.InvalidAddressFormat} \n Ввод: {input}");

        var parts = input.Split(',');
        if (parts.Length < 5 || parts.Length > 6)
            throw new FormatException($"{ErrorMessages.InvalidAddressFormat} \n Ввод: {input}");

        var address = new CreateAddress
        {
            RawInput = input.Trim(),
            Country = parts[0].Trim(),
            Region = parts[1].Trim(),
            City = parts[2].Trim(),
            Street = parts[3].Trim(),
            HouseNumber = parts[4].Trim()
        };

        if (parts.Length == 6)
            address.OfficeOrRoom = parts[5].Trim();
        else if (houseAndApartmentPattern.IsMatch(address.HouseNumber))
        {
            address.OfficeOrRoom = address.HouseNumber.Split("-")[1].Trim();
            address.HouseNumber = address.HouseNumber.Split("-")[0].Trim();
        }
        else
            throw new FormatException($"{ErrorMessages.InvalidAddressFormat} \n {ErrorMessages.InvalidHouseNumber} \n Ввод: {input}");

        if (
            !countryPattern.IsMatch(address.Country) ||
            !regionPattern.IsMatch(address.Region) ||
            !cityPattern.IsMatch(address.City) ||
            !streetPattern.IsMatch(address.Street) ||
            !housePattern.IsMatch(address.HouseNumber) ||
            (address.OfficeOrRoom != null && !additionalInfoPattern.IsMatch(address.OfficeOrRoom)))
        {
            throw new FormatException($"{ErrorMessages.InvalidAddressFormat} \n Ввод: {input}");
        }

        return new ParsedAddress()
        {
            Id = Guid.NewGuid(),
            RawInput = address.RawInput,
            Country = address.Country,
            Region = address.Region,
            City = address.City,
            Street = address.Street,
            HouseNumber = address.HouseNumber,
            OfficeOrRoom = address.OfficeOrRoom
        };
    }
}
