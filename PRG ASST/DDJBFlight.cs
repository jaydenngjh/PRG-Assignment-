//==========================================================
// Student Number: S10268227
// Student Name: Justine Kyle Supan
// Partner Name: Jayden Ng
//==========================================================

class DDJBFlight : Flight
{
    public double RequestFee { get; set; }
    public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime, status)
    {
        RequestFee = 300;
    }
    public double CalculateFees()
    {
        return base.CalculateFees() + RequestFee;
    }
    public override string ToString()
    {
        return base.ToString() + $"Request Fee: {RequestFee}";
    }
}

