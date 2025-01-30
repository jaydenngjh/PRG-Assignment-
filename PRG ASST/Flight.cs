//==========================================================
// Student Number: S10268227
// Student Name: Justine Kyle Supan
// Partner Name: Jayden Ng
//==========================================================

namespace PRG_ASST
{
    public class Flight : IComparable<Flight>
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }


        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }

        public double CalculateFees()
        {
            double fee = 0;
            if (Destination == "Singapore (SIN)") // Arriving flight
            {
                fee += 500;
            }

            else if (Origin == "Singapore (SIN)") // Departing flight
            {
                fee += 800;
            }
            return fee;
        }

        public override string ToString()
        {
            return $"Flight: {FlightNumber}, Origin: {Origin}, Destination: {Destination}, ExpectedTime: {ExpectedTime}, Status: {Status}";
        }

        public int CompareTo(Flight other)
        {
            return this.ExpectedTime.CompareTo(other.ExpectedTime);
        }

    }
}
