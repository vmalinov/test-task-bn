using AddressParser.Application.DTOs;
using AddressParser.Domain.Entities;

namespace AddressParser.Application.Interfaces;

public interface IAddressParser
{
    ParsedAddress Parse(string input);
    string CreateRawAddress(ParsedAddress address);
}

