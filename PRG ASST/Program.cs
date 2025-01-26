using PRG_ASST;

// load the flights.csv file
// create the Flight objects based on the data loaded
// add the Flight objects into a Dictionary

Dictionary<string, Flight> flights = new Dictionary<string, Flight>();

void LoadFlights(string filePath)
{
    var lines = File.ReadAllLines("flights.csv").Skip(1);
    foreach (var line in lines)
    {
        var parts = line.Split(',');
        string flightNumber = parts[0];
        string origin = parts[1];
        string destination = parts[2];
        DateTime expectedTime = DateTime.Parse(parts[3]);
        string status = parts.Length > 4 ? parts[4] : null;  // if length more than 4, assign parts[4] else assign null

        Flight flight = new Flight(flightNumber, origin, destination, expectedTime, status);

        flights[flightNumber] = flight;
    }
    Console.WriteLine($"{flights.Count} Flights Loaded!");
}




// Task 3: List all flights //


//void ListAllFlights()
//{
//    Console.WriteLine("=============================================");
//    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
//    Console.WriteLine("=============================================");
//    foreach (var flight in terminal.Flights.Values)
//    {
//        Console.WriteLine(flight);
//    }
//}

