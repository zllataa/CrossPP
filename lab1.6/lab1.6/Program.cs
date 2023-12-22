using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<BusSchedule> busSchedules = new List<BusSchedule>();

    static void Main()
    {
        bool exit = false;

        do
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Додати рейс");
            Console.WriteLine("2. Видалити рейс");
            Console.WriteLine("3. Знайти рейс за номером");
            Console.WriteLine("4. Сортування за номером рейсу");
            Console.WriteLine("5. Сортування за часом відправлення");
            Console.WriteLine("6. Сортування за кількістю вільних місць");
            Console.WriteLine("7. Вивести всі рейси");
            Console.WriteLine("8. Вийти");

            Console.Write("Введіть номер опції: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddBus();
                    break;
                case "2":
                    RemoveBus();
                    break;
                case "3":
                    FindBusByNumber();
                    break;
                case "4":
                    SortByBusNumber();
                    break;
                case "5":
                    SortByDepartureTime();
                    break;
                case "6":
                    SortByAvailableSeats();
                    break;
                case "7":
                    DisplayAllBuses();
                    break;
                case "8":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }

        } while (!exit);
    }

    static void AddBus()
    {
        Console.WriteLine("\nДодавання нового рейсу:");
        Console.Write("Номер рейсу: ");
        string number = Console.ReadLine();
        Console.Write("Час відправлення: ");
        string departureTime = Console.ReadLine();
        Console.Write("Дні тижня: ");
        string days = Console.ReadLine();
        Console.Write("Кількість вільних місць: ");
        int availableSeats = int.Parse(Console.ReadLine());
        Console.Write("Маршрут (назва станції, час прибуття, через кому): ");
        string route = Console.ReadLine();

        busSchedules.Add(new BusSchedule(number, departureTime, days, availableSeats, route));
        Console.WriteLine("Рейс додано успішно.");
    }

    static void RemoveBus()
    {
        Console.Write("\nВведіть номер рейсу, який потрібно видалити: ");
        string number = Console.ReadLine();

        BusSchedule busToRemove = busSchedules.Find(bus => bus.Number == number);

        if (busToRemove != null)
        {
            busSchedules.Remove(busToRemove);
            Console.WriteLine($"Рейс {number} видалено успішно.");
        }
        else
        {
            Console.WriteLine($"Рейс з номером {number} не знайдено.");
        }
    }

    static void FindBusByNumber()
    {
        Console.Write("\nВведіть номер рейсу, який потрібно знайти: ");
        string number = Console.ReadLine();

        BusSchedule foundBus = busSchedules.Find(bus => bus.Number == number);

        if (foundBus != null)
        {
            Console.WriteLine(foundBus);
        }
        else
        {
            Console.WriteLine($"Рейс з номером {number} не знайдено.");
        }
    }

    static void SortByBusNumber()
    {
        busSchedules = busSchedules.OrderBy(bus => bus.Number).ToList();
        Console.WriteLine("\nРейси відсортовано за номером:");
        DisplayAllBuses();
    }

    static void SortByDepartureTime()
    {
        busSchedules = busSchedules.OrderBy(bus => bus.DepartureTime).ToList();
        Console.WriteLine("\nРейси відсортовано за часом відправлення:");
        DisplayAllBuses();
    }

    static void SortByAvailableSeats()
    {
        busSchedules = busSchedules.OrderBy(bus => bus.AvailableSeats).ToList();
        Console.WriteLine("\nРейси відсортовано за кількістю вільних місць:");
        DisplayAllBuses();
    }

    static void DisplayAllBuses()
    {
        Console.WriteLine("\nУсі рейси:");
        foreach (var bus in busSchedules)
        {
            Console.WriteLine(bus);
        }
    }
}

class BusSchedule
{
    public string Number { get; set; }
    public string DepartureTime { get; set; }
    public string Days { get; set; }
    public int AvailableSeats { get; set; }
    public string Route { get; set; }

    public BusSchedule(string number, string departureTime, string days, int availableSeats, string route)
    {
        Number = number;
        DepartureTime = departureTime;
        Days = days;
        AvailableSeats = availableSeats;
        Route = route;
    }

    public override string ToString()
    {
        return $"Номер: {Number}, Час відправлення: {DepartureTime}, Дні тижня: {Days}, Вільних місць: {AvailableSeats}, Маршрут: {Route}";
    }
}
