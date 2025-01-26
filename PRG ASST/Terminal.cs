using System;
using System.Collections.Generic;

public class Terminal
{
    public string TerminalName { get; set; }
    private Dictionary<string, Airline> Airlines { get; set; }
    private Dictionary<string, BoardingGate> BoardingGates { get; set; }

    public Terminal(string terminalName)
    {
        TerminalName = terminalName;
        Airlines = new Dictionary<string, Airline>();
        BoardingGates = new Dictionary<string, BoardingGate>();
    }

    public bool AddAirline(Airline airline)
    {
        if (Airlines.ContainsKey(airline.Code))
            return false;

        Airlines.Add(airline.Code, airline);
        return true;
    }

    public bool AddBoardingGate(BoardingGate boardingGate)
    {
        if (BoardingGates.ContainsKey(boardingGate.GateName))
            return false;

        BoardingGates.Add(boardingGate.GateName, boardingGate);
        return true;
    }

    public Airline GetAirlineFromFlight(Flight flight)
    {
        foreach (var airline in Airlines.Values)
        {
            if (airline.Flights.ContainsKey(flight.FlightNumber))
                return airline;
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