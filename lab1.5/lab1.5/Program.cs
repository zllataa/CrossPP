using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        List<Autobus> autobuses = new List<Autobus>();

        // Додайте деякі приклади рейсів для тестування
        autobuses.Add(new Autobus("101", "08:00", "Пн Вт Ср", 20, "Харків 08:00, Полтава 10:00, Київ 13:00"));
        autobuses.Add(new Autobus("102", "10:30", "Сб Нд", 15, "Харків 10:30, Полтава 12:30, Київ 15:30"));
        // Додайте ще рейси за необхідністю

        // Сортування за номером рейсу
        autobuses = autobuses.OrderBy(x => x.Number).ToList();
        DisplayAutobuses(autobuses);

        // Сортування за часом відправлення
        autobuses = autobuses.OrderBy(x => x.DepartureTime).ToList();
        DisplayAutobuses(autobuses);

        // Сортування за кількістю вільних місць
        autobuses = autobuses.OrderBy(x => x.AvailableSeats).ToList();
        DisplayAutobuses(autobuses);

        // Використання регулярного виразу для знаходження ранкових рейсів у вихідні дні з Харкова до Києва з зупинкою в Полтаві
        string pattern = @"^[0-9]{2}:[0-5][0-9]$"; // Регулярний вираз для часу в форматі HH:mm
        var morningBuses = autobuses
            .Where(x => Regex.IsMatch(x.DepartureTime, pattern) && x.Days.Contains("Сб") && x.Days.Contains("Нд") && x.Route.Contains("Харків") && x.Route.Contains("Полтава") && x.Route.Contains("Київ"))
            .ToList();

        Console.WriteLine("\nРанкові рейси у вихідні дні з Харкова до Києва з зупинкою в Полтаві:");
        DisplayAutobuses(morningBuses);

        Console.ReadLine();
    }

    static void DisplayAutobuses(List<Autobus> autobuses)
    {
        Console.WriteLine("\nРозклад автостанції:");
        foreach (var autobus in autobuses)
        {
            Console.WriteLine(autobus);
        }
    }
}

class Autobus
{
    public string Number { get; set; }
    public string DepartureTime { get; set; }
    public string Days { get; set; }
    public int AvailableSeats { get; set; }
    public string Route { get; set; }

    public Autobus(string number, string departureTime, string days, int availableSeats, string route)
    {
        Number = number;
        DepartureTime = departureTime;
        Days = days;
        AvailableSeats = availableSeats;
        Route = route;
    }

    public override string ToString()
    {
        return $"{Number} - {DepartureTime}, {Days}, Вільних місць: {AvailableSeats}, Маршрут: {Route}";
    }
}
