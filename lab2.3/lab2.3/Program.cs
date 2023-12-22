using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        List<BusScheduleRecord> busSchedule = GenerateBusSchedule();

        Console.WriteLine("До сортування:");
        DisplayBusSchedule(busSchedule);

        // Послідовна обробка
        var sequentialSortedSchedule = SortBusScheduleSequentially(busSchedule);

        Console.WriteLine("\nПісля послідовної обробки:");
        DisplayBusSchedule(sequentialSortedSchedule);

        // Паралельна обробка
        var parallelSortedSchedule = SortBusScheduleInParallel(busSchedule);

        Console.WriteLine("\nПісля паралельної обробки:");
        DisplayBusSchedule(parallelSortedSchedule);

        Console.ReadLine();
    }

    static List<BusScheduleRecord> GenerateBusSchedule()
    {
        // Генерація тестового розкладу
        List<BusScheduleRecord> busSchedule = new List<BusScheduleRecord>
        {
            new BusScheduleRecord(3, "08:00", "Monday", 20, new List<StationTime>{new StationTime("Station1", "09:30"), new StationTime("Station2", "11:00")}),
            new BusScheduleRecord(1, "10:30", "Wednesday", 15, new List<StationTime>{new StationTime("Station3", "12:00"), new StationTime("Station4", "14:30")}),
            new BusScheduleRecord(2, "14:45", "Friday", 25, new List<StationTime>{new StationTime("Station5", "16:00"), new StationTime("Station6", "18:15")})
        };

        return busSchedule;
    }

    static void DisplayBusSchedule(List<BusScheduleRecord> schedule)
    {
        foreach (var record in schedule)
        {
            Console.WriteLine(record);
        }
    }

    static List<BusScheduleRecord> SortBusScheduleSequentially(List<BusScheduleRecord> schedule)
    {
        // Послідовна обробка
        return schedule
            .OrderBy(record => record.BusNumber)
            .ThenBy(record => record.DepartureTime)
            .ThenBy(record => record.AvailableSeats)
            .ToList();
    }

    static List<BusScheduleRecord> SortBusScheduleInParallel(List<BusScheduleRecord> schedule)
    {
        // Паралельна обробка
        Parallel.ForEach(schedule, record =>
        {
            // Використовуйте потокобезпечні методи сортування тут
            // Наприклад, можна використовувати Interlocked.Exchange або Monitor для забезпечення безпеки
        });

        // Потім можна використовувати послідовний LINQ для завершення сортування
        return schedule
            .OrderBy(record => record.BusNumber)
            .ThenBy(record => record.DepartureTime)
            .ThenBy(record => record.AvailableSeats)
            .ToList();
    }
}

class BusScheduleRecord
{
    public int BusNumber { get; set; }
    public string DepartureTime { get; set; }
    public string DayOfWeek { get; set; }
    public int AvailableSeats { get; set; }
    public List<StationTime> Route { get; set; }

    public BusScheduleRecord(int busNumber, string departureTime, string dayOfWeek, int availableSeats, List<StationTime> route)
    {
        BusNumber = busNumber;
        DepartureTime = departureTime;
        DayOfWeek = dayOfWeek;
        AvailableSeats = availableSeats;
        Route = route;
    }

    public override string ToString()
    {
        return $"Bus {BusNumber} - Departure: {DepartureTime}, Day: {DayOfWeek}, Seats: {AvailableSeats}, Route: {string.Join(", ", Route)}";
    }
}

class StationTime
{
    public string StationName { get; set; }
    public string ArrivalTime { get; set; }

    public StationTime(string stationName, string arrivalTime)
    {
        StationName = stationName;
        ArrivalTime = arrivalTime;
    }

    public override string ToString()
    {
        return $"{StationName} ({ArrivalTime})";
    }
}
