
namespace PRG_ASST
{
    class NORMFlight : Flight
    {
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime, status)
        {
            
        }

        public double CalculateFees()
        {
            return base.CalculateFees() + 300;
        }

        public override string ToString()
        {
            return base.ToString(); 
        }
    }
}
