//==========================================================
// Student Number: S10268227
// Student Name: Justine Kyle Supan
// Partner Name: Jayden Ng
//==========================================================

using PRG_ASST;
using System.Dynamic;


// load the flights.csv file
// create the Flight objects based on the data loaded
// add the Flight objects into a Dictionary

Dictionary<string, Flight> FlightsDict = new Dictionary<string, Flight>();

string FilePath = "flights.csv";

// Load Flights // TASK 2
void LoadFlights(string filePath)
{
    using (StreamReader reader = new StreamReader(filePath))
    {
        // Skip the header line
        reader.ReadLine();

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] data = line.Split(',');
            string flightNumber = data[0];
            string origin = data[1];
            string destination = data[2];
            DateTime expectedTime = DateTime.Parse(data[3]);
            string status = data.Length > 4 ? data[4] : null; // if length more than 4, assign parts[4] else assign null
            string airlineCode = flightNumber.Substring(0, 2);

            Flight flight = null;

            if (status == "NORM")
            {
                flight = new NORMFlight(flightNumber, origin, destination, expectedTime, status);
            }
            else if (status == "DDJB")
            {
                flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, status);
            }
            else if (status == "CFFT")
            {
                flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, status);
            }
            else if (status == "LWTT")
            {
                flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, status);
            }

            if (flight != null)
            {
                FlightsDict[flightNumber] = flight;
                airline.AddFlight(flight);
            }
        }
    }
}


// List all Flights // TASK 3
void ListAllFlights() 
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Number   " +
        "Airline Name           " +
        "Origin                 " +
        "Destination            " +
        "Expected Departure/Arrival Time");


    foreach (var flight in FlightsDict.Values)
    {
        // Extract the airline code from the flight number (first 2 characters)
        string airlineCode = flight.FlightNumber.Substring(0, 2);
        if (Airlines.TryGetValue(airlineCode, out var airline))
        {
            Console.WriteLine($"{flight.FlightNumber,-15} {airline.Name,-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
        }
    }
}

// Create Dict for boarding gates
Dictionary<string, BoardingGate> BoardingGatesDict = new Dictionary<string, BoardingGate>();

// Load Boarding Gates // TASK 4
void LoadBoardingGates(string filePath) 
{
    using (StreamReader reader = new StreamReader(filePath))
    {
        // Skip the header line
        reader.ReadLine();

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var parts = line.Split(',');
            if (parts.Length < 4) // Ensure there are enough columns
            {
                Console.WriteLine($"Skipping invalid line: {line}");
                continue;
            }

            string gateName = parts[0];
            bool supportsDDJB = bool.Parse(parts[1]);
            bool supportsCFFT = bool.Parse(parts[2]);
            bool supportsLWTT = bool.Parse(parts[3]);

            // Create a new BoardingGate object
            BoardingGate boardingGate = new BoardingGate(gateName, supportsDDJB, supportsCFFT, supportsLWTT);

            // Add the boarding gate to the dictionary
            BoardingGatesDict[gateName] = boardingGate;
        }
    }
    Console.WriteLine($"{BoardingGatesDict.Count} Boarding Gates Loaded!");
}


// Dictionary to store the assignments
Dictionary<string, string> flightBoardingGateAssignments = new Dictionary<string, string>();

// Assign a Boarding Gate // TASK 5
void AssignBoardingGate() 
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");

    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine();

    if (!FlightsDict.ContainsKey(flightNumber))
    {
        Console.WriteLine("Flight Number does not exist!");
        return;
    }

    Flight flight = FlightsDict[flightNumber];
    while (true)
    {
        Console.Write("Enter Boarding Gate Name: ");
        string boardingGateName = Console.ReadLine();

        if (!BoardingGatesDict.ContainsKey(boardingGateName))
        {
            Console.WriteLine("Boarding Gate does not exist!");
            return;
        }

        BoardingGate boardingGate = BoardingGatesDict[boardingGateName];

        // Check if the boarding gate is already assigned to another flight
        if (flightBoardingGateAssignments.ContainsValue(boardingGateName))
        {
            Console.WriteLine($"Boarding Gate {boardingGateName} is already assigned to Flight {boardingGate.Flight.FlightNumber}.");
            return;
        }
    
        // Assign the boarding gate to the flight
        flightBoardingGateAssignments[flightNumber] = boardingGateName;

        Console.WriteLine($"Flight Number: {flight.FlightNumber}");
        Console.WriteLine($"Origin: {flight.Origin}");
        Console.WriteLine($"Destination: {flight.Destination}");
        Console.WriteLine($"Expected Time: {flight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
        Console.WriteLine($"Special Request Code: {flight.Status ?? "None"}");
        Console.WriteLine($"Boarding Gate Name: {boardingGateName}");
        Console.WriteLine($"Supports DDJB: {boardingGate.SupportsDDJB}");
        Console.WriteLine($"Supports CFFT: {boardingGate.SupportsCFFT}");
        Console.WriteLine($"Supports LWTT: {boardingGate.SupportsLWTT}");

        Console.Write("Would you like to update the status of the flight? (Y/N): ");
        string updateStatus = Console.ReadLine().ToUpper();
        if (updateStatus == "Y")
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.Write("Please select the new status of the flight: ");
            if (int.TryParse(Console.ReadLine(), out int option))
            {
                if (option == 1)
                {
                    flight.Status = "Delayed";
                }
                else if (option == 2)
                {
                    flight.Status = "Boarding";
                }
                else if (option == 3)
                {
                    flight.Status = "On Time";
                }
                else
                {
                    Console.WriteLine("Invalid option. Status not updated.");
                }
                Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {boardingGateName}!");
            }
            else
            {
                return;
            }
        } 
    }   
}

// Create new Flight // TASK 6
void CreateNewFlight()
{
    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine();
    Console.Write("Enter Origin: ");
    string origin = Console.ReadLine();
    Console.Write("Enter Destination: ");
    string destination = Console.ReadLine();
    Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
    DateTime expectedTime = DateTime.Parse(Console.ReadLine());
    Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
    string status = Console.ReadLine();
    Console.WriteLine("Boarding Gate Name: ");
    string boardingGateName = Console.ReadLine();

    string airlineCode = flightNumber.Substring(0, 2);

    Flight flight1 = new Flight(flightNumber, origin, destination, expectedTime, status);

    // Add the flight to the FlightsDict dictionary
    FlightsDict[flightNumber] = flight1;


    // Append flight to flights.csv
    using (StreamWriter sw = new StreamWriter("flights.csv", true))
    {
        sw.WriteLine($"{flightNumber},{origin},{destination},{expectedTime},{status}");
    }

    Console.WriteLine($"Flight {flightNumber} has been added!");
    Console.WriteLine("Would you like to add another flight? (Y/N)");
    string addAnother = Console.ReadLine().ToUpper();

    if (addAnother == "Y")
    {
        CreateNewFlight();
    }
    else
    {
        return;
    }
}


// Display Flight Schedule in order of Time // TASK 9
void DisplayFlightSchedule()
{
    Console.WriteLine("=============================================\r\n" +
        "Flight Schedule for Changi Airport Terminal 5\r\n" +
        "=============================================\r\n" +
        "Flight Number   Airline Name           Origin                 Destination            " +
        "Expected Departure/Arrival Time     Status          Boarding Gate\r\n");

    // Convert the dictionary values to a list and sort it
    List<Flight> sortedFlights = new List<Flight>(FlightsDict.Values);
    sortedFlights.Sort();
    
    foreach (var flight in sortedFlights)
    {

        Console.WriteLine(flight);
        
    }

}





// MAIN LOOP //

while (true)
{
    LoadFlights(FilePath);
    Console.WriteLine("Loading Airlines...");
    Console.WriteLine($"66 Airlines Loaded!");
    Console.WriteLine("Loading Boarding Gates...");
    Console.WriteLine($"10 Boarding Gates Loaded!");
    Console.WriteLine("Loading Flights...");
    Console.WriteLine($"1 Flights Loaded!");
    Console.WriteLine("");
    Console.WriteLine("");

    Console.WriteLine("=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("0. Exit");
    Console.WriteLine("");
    Console.Write("Please select your option: ");
    string option = Console.ReadLine();

    if (option == "1")
    {
        ListAllFlights(); // Task 3
        break;
    }
    else if (option == "2")
    {
        ListBoardingGates(); // Task 4
        break;
    }
    else if (option == "3")
    {
        AssignBoardingGate(); // Task 5
        break;
    }
    else if (option == "4")
    {
        CreateNewFlight(); // Task 6
        break;
    }
    else if (option == "5")
    {
        DisplayAirlineFlights(); // Task 7
        break;
    }
    else if (option == "6")
    {
        ModifyFlights(); // Task 8
        break;
    }
    else if (option == "7")
    {
        DisplayFlightSchedule(); // Task 9
        break;
    }
    else if (option == "0")
    {
        break;
    }

}
