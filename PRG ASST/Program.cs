//==========================================================
// Student Number: S10268227
// Student Name: Justine Kyle Supan
// Partner Name: Jayden Ng
//==========================================================


//Start of main program
Terminal terminal = new Terminal("Changi Airport Terminal 5");
BoardingGate boardingGate = new BoardingGate("", false, false, false);
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
Console.WriteLine("8. Display Total Fees Per Airline");
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
    DisplayFullFlightDetails(terminal, boardingGate);
}
else if (mainOption == "6")
{
    ModifyFlightDetails(terminal, boardingGate);
}
else if (mainOption == "7")
{
    DisplayFlightSchedule(terminal, boardingGate);
}
else if (mainOption == "8")
{
    DisplayTotalFees(terminal, boardingGate);
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
            string status = "Scheduled";

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
    Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}{"Expected Departure/Arrival Time"}");

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
    Console.WriteLine($"{"Gate Name",-16}{"CFFT",-21}{"DDJB",-21}{"LWTT",-7}");

    foreach (var gate in terminal.BoardingGates.Values)
    {
        Console.WriteLine($"{gate.GateName,-16}{gate.SupportsCFFT,-21}{gate.SupportsDDJB,-21}{gate.SupportsLWTT,-7}");
    }
}


//BASIC FEATURE 5: Assign a Boarding Gate to a Flight (JUSTINE)
static void AssignBoardingGate(Terminal terminal)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");

    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine();

    if (!terminal.Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Flight Number not found.");
        return;
    }

    Flight flight = terminal.Flights[flightNumber];

    string specialCode;
    if (flight is CFFTFlight)
    {
        specialCode = "CFFT";
    }
    else if (flight is DDJBFlight)
    {
        specialCode = "DDJB";
    }
    else if (flight is LWTTFlight)
    {
        specialCode = "LWTT";
    }
    else
    {
        specialCode = "None";
    }

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
        var assignedGate = terminal.BoardingGates[boardingGateName];

        if (assignedGate.Flight != null)
        {
            Console.WriteLine($"Boarding Gate {boardingGateName} is already assigned to Flight {assignedGate.Flight.FlightNumber}.");
            return;
        }

    }

    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Origin: {flight.Origin}");
    Console.WriteLine($"Destination: {flight.Destination}");
    Console.WriteLine($"Expected Time: {flight.ExpectedTime:dd/MM/yyyy hh:mm:ss tt}");
    Console.WriteLine($"Special Request Code: {specialCode}");
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



//BASIC FEATURE 6: Create Flight
static void CreateFlight(Terminal terminal)
{
    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine();
    if (terminal.Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Flight Number already exists.");
        return;
    }

    Console.Write("Enter Origin: ");
    string origin = Console.ReadLine();
    Console.Write("Enter Destination: ");
    string destination = Console.ReadLine();
    Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
    DateTime expectedTime = DateTime.Parse(Console.ReadLine());
    Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
    string status = Console.ReadLine();
    Console.Write("Boarding Gate Name: ");
    string boardingGateName = Console.ReadLine();

    string airlineCode = flightNumber.Substring(0, 2);

    Flight flightObj = new Flight(flightNumber, origin, destination, expectedTime, status);

    // Add the flight to the FlightsDict dictionary
    terminal.Flights[flightNumber] = flightObj;


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
static void DisplayFullFlightDetails(Terminal terminal, BoardingGate boardingGate)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Airline Code\tAirline Name");
    foreach (var airline in terminal.Airlines.Values)
    {
        Console.WriteLine($"{airline.Code}\t\t{airline.Name}");
    }

    Console.Write("Enter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (!terminal.Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("Invalid Airline Code.");
        return;
    }

    Airline selectedAirline = terminal.Airlines[airlineCode];

    Console.WriteLine("=============================================");
    Console.WriteLine($"List of Flights for {selectedAirline.Name}");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-22} {"Origin",-22} {"Destination",-22} {"Expected Departure/Arrival Time"}");
    foreach (var flight in terminal.Flights.Values)
    {
        string airline = terminal.GetAirlineFromFlight(flight).Name;
        if (airline == selectedAirline.Name)
        {
            Console.WriteLine($"{flight.FlightNumber,-16}{airline,-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime}");
        }
    }
}


//BASIC FEATURE 8: Modify Flight Details
static void ModifyFlightDetails(Terminal terminal, BoardingGate boardingGate)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Airline Code\tAirline Name");
    foreach (var airline in terminal.Airlines.Values)
    {
        Console.WriteLine($"{airline.Code}\t\t{airline.Name}");
    }

    Console.Write("Enter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (!terminal.Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("Invalid Airline Code.");
        return;
    }

    Airline selectedAirline = terminal.Airlines[airlineCode];

    Console.WriteLine($"List of Flights for {selectedAirline.Name}");
    Console.WriteLine("Flight Number\tAirline Name\tOrigin\tDestination\tExpected Departure/Arrival Time");
    foreach (var flight in terminal.Flights.Values)
    {
        string airline = terminal.GetAirlineFromFlight(flight).Name;
        if (airline == selectedAirline.Name)
        {
            Console.WriteLine($"{flight.FlightNumber}\t{airline}\t{flight.Origin}\t{flight.Destination}\t{flight.ExpectedTime}");
        }
    }

    Console.Write("Choose an existing Flight to modify or delete: ");
    string flightNumber = Console.ReadLine().ToUpper();

    if (!terminal.Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Invalid Flight Number.");
        return;
    }

    Flight selectedFlight = terminal.Flights[flightNumber];

    Console.WriteLine("1. Modify Flight");
    Console.WriteLine("2. Delete Flight");
    Console.Write("Choose an option: ");
    int option = int.Parse(Console.ReadLine());

    if (option == 1)
    {
        Console.WriteLine("1. Modify Basic Information");
        Console.WriteLine("2. Modify Status");
        Console.WriteLine("3. Modify Special Request Code");
        Console.WriteLine("4. Modify Boarding Gate");
        Console.Write("Choose an option: ");
        int modifyOption = int.Parse(Console.ReadLine());

        switch (modifyOption)
        {
            case 1:
                Console.Write("Enter new Origin: ");
                string newOrigin = Console.ReadLine();
                Console.Write("Enter new Destination: ");
                string newDestination = Console.ReadLine();
                Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                DateTime newExpectedTime = DateTime.Parse(Console.ReadLine());

                selectedFlight.Origin = newOrigin;
                selectedFlight.Destination = newDestination;
                selectedFlight.ExpectedTime = newExpectedTime;

                Console.WriteLine("Flight updated!");
                break;

            case 2:
                Console.Write("Enter new Status: ");
                string newStatus = Console.ReadLine();
                selectedFlight.Status = newStatus;
                Console.WriteLine("Flight status updated!");
                break;

            case 3:
                Console.Write("Enter new Special Request Code: ");
                string newSpecialRequestCode = Console.ReadLine().ToUpper();
                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (newSpecialRequestCode == "DDJB")
                    {
                        selectedFlight = new DDJBFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status);

                    }
                    else if (newSpecialRequestCode == "CFFT")
                    {
                        selectedFlight = new CFFTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status);

                    }
                    else if (newSpecialRequestCode == "LWTT")
                    {
                        selectedFlight = new LWTTFlight(selectedFlight.FlightNumber, selectedFlight.Origin, selectedFlight.Destination, selectedFlight.ExpectedTime, selectedFlight.Status);

                    }
                    else
                    {
                        Console.WriteLine("Invalid Special Request Code.");
                    }

                }
                break;

            case 4:
                Console.Write("Enter new Boarding Gate: ");
                string newBoardingGate = Console.ReadLine();
                boardingGate.GateName = newBoardingGate;
                Console.WriteLine("Flight boarding gate updated!");
                break;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
    else if (option == 2)
    {
        Console.Write("Are you sure you want to delete this flight? [Y/N]: ");
        string confirm = Console.ReadLine().ToUpper();

        if (confirm == "Y")
        {
            terminal.Flights.Remove(flightNumber);
            Console.WriteLine("Flight deleted successfully!");
        }
        else
        {
            Console.WriteLine("Deletion cancelled.");
        }
    }
    else
    {
        Console.WriteLine("Invalid option.");
    }
    foreach (var flight in terminal.Flights.Values)
    {
        string airline = terminal.GetAirlineFromFlight(flight).Name;
        // Display updated flight details
        Console.WriteLine("Updated Flight Details:");
        Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
        Console.WriteLine($"Airline Name: {airline}");
        Console.WriteLine($"Origin: {selectedFlight.Origin}");
        Console.WriteLine($"Destination: {selectedFlight.Destination}");
        Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime}");
        Console.WriteLine($"Status: {selectedFlight.Status}");

        string requestCode;
        if (selectedFlight is CFFTFlight)
        {
            requestCode = "CFFT";
        }
        else if (selectedFlight is DDJBFlight)
        {
            requestCode = "DDJB";
        }
        else if (selectedFlight is LWTTFlight)
        {
            requestCode = "LWTT";
        }
        else
        {
            requestCode = "None";
        }

        Console.WriteLine($"Special Request Code: {requestCode}");

        if (boardingGate.GateName != "")
        {
            Console.WriteLine($"Boarding Gate: {boardingGate.GateName}");
        }
        else
        {
            Console.WriteLine($"Boarding Gate: Unassigned");
        }
        break;
    }
}


//BASIC FEATURE 9: Display Flight Schedule
static void DisplayFlightSchedule(Terminal terminal, BoardingGate boardingGate)
{
    Console.WriteLine("=============================================\r\n" +
        "Flight Schedule for Changi Airport Terminal 5\r\n" +
        "=============================================\r\n" +
        "Flight Number   Airline Name           Origin                 Destination            " +
        "Expected Departure/Arrival Time    Status          Boarding Gate");

    // Convert the dictionary values to a list and sort it
    List<Flight> sortedFlights = new List<Flight>(terminal.Flights.Values);
    sortedFlights.Sort();

    foreach (var flight in sortedFlights)
    {
        string airline = terminal.GetAirlineFromFlight(flight).Name;
        if (boardingGate.GateName == "")
        {
            boardingGate.GateName = "Unassigned";
        }


        Console.WriteLine($"{flight.FlightNumber,-16}{airline,-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime,-35}{flight.Status,-16}{boardingGate.GateName}");
    }
}



// ADVANCED FEATURE 2: Display Total Fees Per Airline
void DisplayTotalFees(Terminal terminal, BoardingGate boardingGate)
{
    // Check if all flights have been assigned boarding gates
    List<Flight> unassignedFlights = terminal.Flights.Values
        .Where(flight => boardingGate.GateName == "Unassigned")
        .ToList();

    if (unassignedFlights.Any())
    {
        Console.WriteLine("The following flights do not have boarding gates assigned:");
        foreach (var flight in unassignedFlights)
        {
            Console.WriteLine("- " + flight.FlightNumber);
        }
        Console.WriteLine("Please assign boarding gates to these flights before running this feature.");
        return;
    }

    double totalFeesAllAirlines = 0;
    double totalDiscountsAllAirlines = 0;

    Console.WriteLine("Please select your option:");
    Console.WriteLine();
    Console.WriteLine("{0,-25} {1,-15} {2,-15} {3,-15} {4,-25}",
        "Airline", "SubTotal", "Discount", "FinalTotal", "Discount Percentage of Total");

    foreach (var airline in terminal.Airlines.Values)
    {
        double subtotalFees = 0;
        double subtotalDiscounts = 0;

        foreach (var flight in terminal.Flights.Values)
        {
            if (terminal.GetAirlineFromFlight(flight).Code == airline.Code)
            {
                double flightFee = flight.CalculateFees();
                subtotalFees += flightFee;

                // Calculate Discounts
                double discount = 0;
                int flightCount = terminal.Flights.Values
                    .Count(f => terminal.GetAirlineFromFlight(f).Code == airline.Code);
                discount += ((double)flightCount / 3) * 350;

                if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour >= 21)
                {
                    discount += 110;
                }

                if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                {
                    discount += 25;
                }

                if (!boardingGate.SupportsDDJB)
                {
                    discount += 50;
                }

                if (!boardingGate.SupportsCFFT)
                {
                    discount += 50;
                }

                if (!boardingGate.SupportsLWTT)
                {
                    discount += 50;
                }

                if (flightCount > 5)
                {
                    discount += flightFee * 0.03;
                }
                subtotalDiscounts += discount;
                //DEBUG
                //Console.WriteLine($"DEBUG: {flight.FlightNumber} - Fee: {flightFee}, Discount: {discount}");
            }
        }

        double finalTotal = subtotalFees - subtotalDiscounts;
        double discountPercentage = subtotalFees > 0 ? (subtotalDiscounts / subtotalFees) * 100 : 0;

        Console.WriteLine("{0,-25} ${1,-14:F2} ${2,-14:F2} ${3,-14:F2} {4,4:F2}%",
            airline.Name,
            subtotalFees,
            subtotalDiscounts,
            finalTotal,
            discountPercentage);

        totalFeesAllAirlines += subtotalFees;
        totalDiscountsAllAirlines += subtotalDiscounts;
    }

    double finalTotalAllAirlines = totalFeesAllAirlines - totalDiscountsAllAirlines;
    double totalDiscountPercentage = (totalDiscountsAllAirlines / totalFeesAllAirlines) * 100;

    Console.WriteLine();
    Console.WriteLine("Terminal 5      Total Fee: ${0:F2}    Discount: ${1:F2}    Final Total: ${2:F2}    Discount Percentage of Total: {3:F2}%",
        totalFeesAllAirlines,
        totalDiscountsAllAirlines,
        finalTotalAllAirlines,
        totalDiscountPercentage);
}
