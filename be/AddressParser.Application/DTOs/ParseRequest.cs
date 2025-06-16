using System.ComponentModel.DataAnnotations;
using AddressParser.Domain.Errors;

namespace AddressParser.Application.DTOs;

public class ParseRequest
{
    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [MaxLength(255, ErrorMessage = ErrorMessages.MaxLengthExceeded)]
    public string RawAddress { get; set; } = default!;
}