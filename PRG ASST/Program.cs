using System;
using System.Collections.Generic;
using System.IO;

//Start of main program
Terminal terminal = new Terminal("Changi Airport Terminal 5");
LoadAirlines(terminal);
LoadBoardingGates(terminal);
LoadFlights(terminal);

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
Console.Write("Please select your option: ");
string mainOption = Console.ReadLine();
if (mainOption == "0")
{
    Console.WriteLine("Goodbye!");
    return;
}
else if (mainOption == "1")
{
    ListAllFlights(terminal);
}
else if (mainOption == "2")
{
    ListBoardingGates(terminal);
}
else if (mainOption == "3")
{
    AssignBoardingGate(terminal);
}
else if (mainOption == "4")
{
    CreateFlight(terminal);
}
else if (mainOption == "5")
{
    DisplayFullFlightDetails(terminal);
}
else if (mainOption == "6")
{
    //ModifyFlightDetails(terminal);
}
else if (mainOption == "7")
{
    DisplayFlightSchedule(terminal);
}
else
{
    Console.WriteLine("Invalid option. Please try again.");
}


//BASIC FEATURE 1.1: Loading Airlines
static void LoadAirlines(Terminal terminal)
{
    Console.WriteLine("Loading Airlines...");
    string line;
    using (StreamReader reader = new StreamReader("airlines.csv"))
    {
        reader.ReadLine(); // Skip header
        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split(',');
            string name = parts[0];
            string code = parts[1];
            Airline airline = new Airline(name, code);
            terminal.AddAirline(airline);
        }
    }
    int NumAirlines = terminal.Airlines.Count;
    
    Console.WriteLine($"{NumAirlines} Airlines Loaded!");
}

//BASIC FEATURE 1.2: Loading Boarding Gates
static void LoadBoardingGates(Terminal terminal)
{
    Console.WriteLine("Loading Boarding Gates...");
    using (StreamReader reader = new StreamReader("boardinggates.csv"))
    {
        reader.ReadLine(); // Skip header

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split(',');
            string gateName = parts[0];
            bool supportsCFFT = bool.Parse(parts[1]);
            bool supportsDDJB = bool.Parse(parts[2]);
            bool supportsLWTT = bool.Parse(parts[3]);
            BoardingGate boardingGate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);
            terminal.AddBoardingGate(boardingGate);
        }
        int Numgates = terminal.BoardingGates.Count;
        Console.WriteLine($"{Numgates} Boarding Gates Loaded!");
    }
}

//BASIC FEATURE 2: Load Flights
static void LoadFlights(Terminal terminal)
{
    Console.WriteLine("Loading Flights...");
    using (StreamReader reader = new StreamReader("flights.csv"))
    {
        reader.ReadLine();

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split(',');
            string flightNumber = parts[0];
            string origin = parts[1];
            string destination = parts[2];
            DateTime expectedTime = DateTime.Parse(parts[3]);
            string specialRequestCode = parts.Length > 3 ? parts[4] : null;
            string status = "Unassigned";

            Flight flight;
            switch (specialRequestCode)
            {
                case "DDJB":
                    flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, status);
                    break;
                case "CFFT":
                    flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, status);
                    break;
                case "LWTT":
                    flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, status);
                    break;
                default:
                    flight = new NORMFlight(flightNumber, origin, destination, expectedTime, status);
                    break;
            }
            terminal.Flights.Add(flightNumber, flight);
        }
    }
    int NumFlights = terminal.Flights.Count;
    Console.WriteLine($"{NumFlights} Flights Loaded!");
}

//BASIC FEATURE 3: List All Flights
static void ListAllFlights(Terminal terminal)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Flights for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number", -16}{"Airline Name", -23}{"Origin", -23}{"Destination", -23}{"Expected Departure/Arrival Time"}");

    foreach (var flight in terminal.Flights.Values)
    {
        string airline = terminal.GetAirlineFromFlight(flight).Name;
        Console.WriteLine($"{flight.FlightNumber,-15} {airline,-22} {flight.Origin,-21}  {flight.Destination,-21}  {flight.ExpectedTime,-7} ");
    }
}



//BASIC FEATURE 4: List Boarding Gates
static void ListBoardingGates(Terminal terminal)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Gate Name", -16}{"CFFT", -21}{"DDJB", -21}{"LWTT", -7}");

    foreach (var gate in terminal.BoardingGates.Values)
    {
        Console.WriteLine($"{gate.GateName, -16}{gate.SupportsCFFT, -21}{gate.SupportsDDJB, -21}{gate.SupportsLWTT, -7}");
    }
}

//BASIC FEATURE 5: Assign a Boarding Gate to a Flight
static void AssignBoardingGate(Terminal terminal)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");

    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine();

    if (!terminal.Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Flight Number does not exist!");
        return;
    }

    Flight flight = terminal.Flights[flightNumber];
    while (true)
    {
        Console.Write("Enter Boarding Gate Name: ");
        string boardingGateName = Console.ReadLine();

        if (!terminal.BoardingGates.ContainsKey(boardingGateName))
        {
            Console.WriteLine("Boarding Gate does not exist!");
            return;
        }

        BoardingGate boardingGate = terminal.BoardingGates[boardingGateName];

        // Check if the boarding gate is already assigned to another flight
        if (terminal.BoardingGates[boardingGateName] != null)
        {
            Console.WriteLine($"Boarding Gate {boardingGateName} is already assigned to Flight {boardingGate.Flight.FlightNumber}.");
            return;
        }

        Console.WriteLine($"Flight Number: {flight.FlightNumber}");
        Console.WriteLine($"Origin: {flight.Origin}");
        Console.WriteLine($"Destination: {flight.Destination}");
        Console.WriteLine($"Expected Time: {flight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
        //Console.WriteLine($"Special Request Code: {terminal.Flights[4] ?? "None"}");
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
                // Assign the boarding gate to the flight
                //terminal.BoardingGates[flightNumber] = boardingGateName;
                Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {boardingGateName}!");
            }
            else
            {
                return;
            }
        }
    }
}

//BASIC FEATURE 6: Create Flight
static void CreateFlight(Terminal terminal)
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
    terminal.Flights[flightNumber] = flight1;


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
        CreateFlight(terminal);
    }
    else
    {
        return;
    }


}

// BASIC FEATURE 7: Display Full Flight Details from an Airline
static void DisplayFullFlightDetails(Terminal terminal)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Display Full Flight Details from an Airline");
    Console.WriteLine("=============================================");

    // List all available airlines
    Console.WriteLine("Available Airlines:");
    foreach (var airline in terminal.Airlines.Values)
    {
        Console.WriteLine($"{airline.Code} - {airline.Name}");
    }

    // Prompt the user to enter the 2-letter airline code
    Console.Write("Enter the 2-letter Airline Code (e.g., SQ, MH): ");
    string airlineCode = Console.ReadLine().ToUpper();

    // Retrieve the airline object
    if (!terminal.Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("Airline not found.");
        return;
    }

    Airline selectedAirline = terminal.Airlines[airlineCode];

    // List all flights for the selected airline
    Console.WriteLine($"Flights for {selectedAirline.Name}:");
    foreach (var flight in terminal.Flights.Values)
    {
        if (flight.FlightNumber.StartsWith(airlineCode))
        {
            Console.WriteLine($"{flight.FlightNumber} - {flight.Origin} to {flight.Destination}");
        }
    }

    // Prompt the user to select a flight number
    Console.Write("Enter the Flight Number: ");
    string flightNumber = Console.ReadLine().ToUpper();

    // Retrieve the flight object
    if  (!selectedAirline.Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Flight not found.");
        return;
    }

    Flight selectedFlight = selectedAirline.Flights[flightNumber];

    // Display full flight details
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Details:");
    Console.WriteLine("=============================================");
    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
    Console.WriteLine($"Airline: {selectedAirline.Name}");
    Console.WriteLine($"Origin: {selectedFlight.Origin}");
    Console.WriteLine($"Destination: {selectedFlight.Destination}");
    Console.WriteLine($"Expected Time: {selectedFlight.ExpectedTime}");
}

//BASIC FEATURE 9: Display Flight Schedule
static void DisplayFlightSchedule(Terminal terminal)
{
    Console.WriteLine("=============================================\r\n" +
        "Flight Schedule for Changi Airport Terminal 5\r\n" +
        "=============================================\r\n" +
        "Flight Number   Airline Name           Origin                 Destination            " +
        "Expected Departure/Arrival Time     Status          Boarding Gate\r\n");

    // Convert the dictionary values to a list and sort it
    List<Flight> sortedFlights = new List<Flight>(terminal.Flights.Values);
    sortedFlights.Sort();

    foreach (var flight in sortedFlights)
    {

        Console.WriteLine(flight);

    }

}
