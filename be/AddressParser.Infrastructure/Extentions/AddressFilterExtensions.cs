using System.Linq.Expressions;
using AddressParser.Application.DTOs;
using AddressParser.Domain.Entities;

namespace AddressParser.Infrastructure.Extensions;

public static class AddressFilterExtensions
{
    public static IQueryable<ParsedAddress> ApplyFilter(this IQueryable<ParsedAddress> query, AddressFilter filter)
    {

        //NOTE: поиск сейчас максимально простой, но можно разбить на элементы и сделать поиск более "жадным"
        // так же можно добавить полнотекстовой индекс в таблицу для более быстрого результата (GIN/GiST)
        if (!string.IsNullOrWhiteSpace(filter.RawInput))
            query = query.Where(a => a.RawInput.Replace(",", "").ToLower().Contains(filter.RawInput.Replace(",", "").ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.Country))
            query = query.Where(a => a.Country.ToLower().Contains(filter.Country.ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.Region))
            query = query.Where(a => a.Region.ToLower().Contains(filter.Region.ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.City))
            query = query.Where(a => a.City.ToLower().Contains(filter.City.ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.Street))
            query = query.Where(a => a.Street.ToLower().Contains(filter.Street.ToLower()));

        if (!string.IsNullOrWhiteSpace(filter.HouseNumber))
            query = query.Where(a => a.HouseNumber.ToLower().Contains(filter.HouseNumber.ToLower()));

        return query;
    }
}
