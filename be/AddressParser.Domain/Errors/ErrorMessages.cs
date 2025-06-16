
namespace AddressParser.Domain.Errors;

public static class ErrorMessages
{
    public const string InvalidAddressFormat = "Неверный формат адреса.";
    public const string InvalidHouseNumber = "Неверный номер дома.";
    public const string RequiredField = "{0} обязательно для заполнения.";
    public const string MaxLengthExceeded = "Длина поля \"{0}\" должна быть меньше или равна {1}.";
    public const string RecordNotFound = "Запись не найдена. Обновите страницу.";
    public const string AlreadyUpdated = "Запись была обновлена другим пользователем. Обновите страницу.";

}