using System;
using System.Collections.Generic;
using System.IO;
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

[Serializable]
class AutostationContainer
{
    private List<AutostationRecord> records;

    public AutostationContainer()
    {
        records = new List<AutostationRecord>();
    }

    public void AddRecord(AutostationRecord record)
    {
        records.Add(record);
    }

    public void RemoveRecord(int index)
    {
        if (index >= 0 && index < records.Count)
            records.RemoveAt(index);
    }

    public void ModifyRecord(int index, AutostationRecord newRecord)
    {
        if (index >= 0 && index < records.Count)
            records[index] = newRecord;
    }

    public IEnumerable<AutostationRecord> GetRecords()
    {
        return records;
    }

    public void Serialize(string fileName)
    {
        string jsonString = JsonSerializer.Serialize(records);
        File.WriteAllText(fileName, jsonString);
    }

    public static AutostationContainer Deserialize(string fileName)
    {
        string jsonString = File.ReadAllText(fileName);
        var deserializedRecords = JsonSerializer.Deserialize<List<AutostationRecord>>(jsonString);

        var container = new AutostationContainer();
        container.records.AddRange(deserializedRecords);

        return container;
    }
}

class Program
{
    static void Main()
    {
        AutostationContainer autostation = new AutostationContainer();

        // Додавання прикладових записів
        autostation.AddRecord(new AutostationRecord
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

        autostation.AddRecord(new AutostationRecord
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

        // Серіалізація
        autostation.Serialize("autostation.json");

        // Десеріалізація
        AutostationContainer deserializedAutostation = AutostationContainer.Deserialize("autostation.json");

        // Вивід записів
        Console.WriteLine("Autostation Records:");
        foreach (var record in deserializedAutostation.GetRecords())
        {
            Console.WriteLine($"Flight Number: {record.FlightNumber}, Departure Time: {record.DepartureTime}, Available Seats: {record.AvailableSeats}");
        }
    }
}
