

namespace PRG_ASST
{
    class LWTTFlight : Flight
    {
        public double RequestFee { get; set; } 

        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime, status)
        {
            RequestFee = 500;
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
}
