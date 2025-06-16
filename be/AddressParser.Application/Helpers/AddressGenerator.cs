public static class AddressGenerator
{
    private static Random _random = new Random();
    private static readonly string[] countries = {"Республика Беларусь", "Беларусь"};
    private static readonly string[] regions = {"Гомельская область", "Минская область", "Витебская область", "Брестская область"};

    private static readonly string[] cities = {"Гомель", "Минск", "Могилёв", "Витебск", "Брест"};
    private static readonly string[] streets = {"ул. Братьев Лизюковых", "ул. Советская", "ул. Кирова", "ул. Пушкина", "ул. Мира"};

    public static string GenerateRandomAddress()
    {
        var country = countries[_random.Next(countries.Length)];
        var region = regions[_random.Next(regions.Length)];
        var city = cities[_random.Next(cities.Length)];
        var street = streets[_random.Next(streets.Length)];
        var houseNumber = _random.Next(1, 200); 
        var apartmentNumber = _random.Next(1, 500);


        bool useShortFormat = _random.NextDouble() > 0.5;

        if (useShortFormat)
        {
            return $"{country}, {region}, г. {city}, {street}, {houseNumber} - {apartmentNumber}";
        }
        else
        {
            return $"{country}, {region}, г. {city}, {street}, д. {houseNumber}, кв. {apartmentNumber}";
        }
    }
}