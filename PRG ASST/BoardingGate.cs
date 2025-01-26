//==========================================================
// Student Number: S10268227
// Student Name: Justine Kyle Supan
// Partner Name: Jayden Ng
//==========================================================

namespace PRG_ASST
{
    public class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsSLWTT { get; set; }
        public Flight Flight { get; set; } // 0..1 relationship with Flight
    
        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsSLWTT)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsSLWTT = supportsSLWTT;
        }
    
        public double CalculateFees()
        {
            // Implement fee calculation logic based on gate features
            return 0.0; // Placeholder
        }
    
        public override string ToString()
        {
            return $"BoardingGate: {GateName}, CFFT: {SupportsCFFT}, DDJB: {SupportsDDJB}, SLWTT: {SupportsSLWTT}";
        }
    }
 }
