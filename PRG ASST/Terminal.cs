//==========================================================
// Student Number: S10268227
// Student Name: Justine Kyle Supan
// Partner Name: Jayden Ng
//==========================================================



public class Terminal
{
    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; set; }
    public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
    public Dictionary<string, BoardingGate> BoardingGates { get; set; }
    public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();

    public Terminal(string terminalName)
    {
        TerminalName = terminalName;
        Airlines = new Dictionary<string, Airline>();
        BoardingGates = new Dictionary<string, BoardingGate>();
        GateFees = new Dictionary<string, double>();
    }

    public bool AddAirline(Airline airline)
    {
        try
        {
            Airlines.Add(airline.Code, airline);
            return true;
        }
        catch
        {
            Console.WriteLine("Airline Addition Unsuccessful. Do try again. ");
            return false;
        }
    }

    public bool AddBoardingGate(BoardingGate boardingGate)
    {
        if (BoardingGates.ContainsKey(boardingGate.GateName))
        {
            return false;
        }
        else
        {
            BoardingGates.Add(boardingGate.GateName, boardingGate);
            return true;
        }
    }

    public Airline GetAirlineFromFlight(Flight flight)
    {
        foreach (KeyValuePair<string, Airline> kvp in Airlines)
        {
            string[] flightno = flight.FlightNumber.Split(' ');
            if (flightno[0] == kvp.Key)
            {
                return kvp.Value;
            }

        }
        return null;
    }

    public void PrintAirlineFees()
    {
        foreach (var airline in Airlines.Values)
        {
            Console.WriteLine($"{airline.Name}: {airline.CalculateFees()}");
        }
    }

    public override string ToString()
    {
        return $"Terminal: {TerminalName}, Airlines: {Airlines.Count}, Boarding Gates: {BoardingGates.Count}";
    }
}
