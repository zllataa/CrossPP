using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

[Serializable]
class AutostationRecord
{
    public int FlightNumber { get; set; }
    public DateTime DepartureTime { get; set; }
    public string[] DaysOfWeek { get; set; } = Array.Empty<string>();
    public int AvailableSeats { get; set; }
    public Dictionary<string, DateTime> Route { get; set; } = new Dictionary<string, DateTime>();
}

class AutostationContainer<T> : IEnumerable<T> where T : AutostationRecord
{
    private LinkedList<T> records = new LinkedList<T>();

    public void AddRecord(T record)
    {
        records.AddLast(record);
    }

    public void RemoveRecord(T record)
    {
        records.Remove(record);
    }

    public void SortByFlightNumber()
    {
        LinkedList<T> sortedRecords = new LinkedList<T>(records.OrderBy(r => r.FlightNumber));
        records = new LinkedList<T>(sortedRecords);
    }

    public void SortByDepartureTime()
    {
        LinkedList<T> sortedRecords = new LinkedList<T>(records.OrderBy(r => r.DepartureTime));
        records = new LinkedList<T>(sortedRecords);
    }

    public void SortByAvailableSeats()
    {
        LinkedList<T> sortedRecords = new LinkedList<T>(records.OrderBy(r => r.AvailableSeats));
        records = new LinkedList<T>(sortedRecords);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return records.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Serialize(string fileName)
    {
        string jsonString = JsonSerializer.Serialize(records);
        File.WriteAllText(fileName, jsonString);
    }

    public static AutostationContainer<T> Deserialize(string fileName)
    {
        string jsonString = File.ReadAllText(fileName);
        var deserializedRecords = JsonSerializer.Deserialize<List<T>>(jsonString);

        var container = new AutostationContainer<T>();
        container.records = new LinkedList<T>(deserializedRecords);

        return container;
    }
}

class Program
{
    static void Main()
    {
        AutostationContainer<AutostationRecord> autostationContainer = new AutostationContainer<AutostationRecord>();

        // Додавання прикладових записів
        autostationContainer.AddRecord(new AutostationRecord
        {
            FlightNumber = 101,
            DepartureTime = new DateTime(2023, 1, 1, 8, 0, 0),
            DaysOfWeek = new string[] { "Monday", "Wednesday", "Friday" },
            AvailableSeats = 20,
            Route = new Dictionary<string, DateTime>
            {
                { "StationA", new DateTime(2023, 1, 1, 8, 0, 0) },
                { "StationB", new DateTime(2023, 1, 1, 10, 0, 0) }
            }
        });

        autostationContainer.AddRecord(new AutostationRecord
        {
            FlightNumber = 102,
            DepartureTime = new DateTime(2023, 1, 2, 9, 0, 0),
            DaysOfWeek = new string[] { "Tuesday", "Thursday" },
            AvailableSeats = 15,
            Route = new Dictionary<string, DateTime>
            {
                { "StationX", new DateTime(2023, 1, 2, 9, 0, 0) },
                { "StationY", new DateTime(2023, 1, 2, 11, 0, 0) }
            }
        });

        // Сортування за номером рейсу
        autostationContainer.SortByFlightNumber();

        // Серіалізація
        autostationContainer.Serialize("autostation.json");

        // Десеріалізація
        AutostationContainer<AutostationRecord> deserializedAutostation = AutostationContainer<AutostationRecord>.Deserialize("autostation.json");

        // Вивід записів
        Console.WriteLine("Autostation Records:");
        foreach (var record in deserializedAutostation)
        {
            Console.WriteLine($"Flight Number: {record.FlightNumber}, Departure Time: {record.DepartureTime}, Available Seats: {record.AvailableSeats}");
        }
    }
}
