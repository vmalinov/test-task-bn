using System.ComponentModel.DataAnnotations;
using AddressParser.Domain.Errors;

namespace AddressParser.Application.DTOs;
//NOTE: Здесь сделана простая валидация через DataAnnotations, но можно использовать FluentValidation
public class CreateAddress
{
    public string RawInput { get; set; } = null!;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [MaxLength(100, ErrorMessage = ErrorMessages.MaxLengthExceeded)]
    public string Country { get; init; } = null!;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [MaxLength(100, ErrorMessage = ErrorMessages.MaxLengthExceeded)]
    public string Region { get; init; } = null!;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [MaxLength(100, ErrorMessage = ErrorMessages.MaxLengthExceeded)]
    public string City { get; init; } = null!;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    [MaxLength(100, ErrorMessage = ErrorMessages.MaxLengthExceeded)]
    public string Street { get; init; } = null!;

    [Required(ErrorMessage = ErrorMessages.RequiredField)]
    public string HouseNumber { get; set; } = null!;

    public string? OfficeOrRoom { get; set; }
}