using System;

public class BoardingGate
{
    public string GateName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsLWTT { get; set; }
    public Flight Flight { get; set; } // 0..1 relationship with Flight

    public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
    {
        GateName = gateName;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsLWTT;
    }

    public double CalculateFees()
    {
        // Implement fee calculation logic based on gate features
        return 0.0; // Placeholder
    }

    public override string ToString()
    {
        return $"BoardingGate: {GateName}, CFFT: {SupportsCFFT}, DDJB: {SupportsDDJB}, SLWTT: {SupportsLWTT}";
    }
}
