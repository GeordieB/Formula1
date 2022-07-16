namespace Formula1.Entities
{
    public class RaceResult
    {
        public int RaceNumber { get; set; }
        public DriverResult FirstDriverResult { get; set; }
        public DriverResult SecondDriverResult { get; set; }

        public RaceResult(int raceNumber, DriverResult firstDriver, DriverResult secondDriver)
        {
            RaceNumber = raceNumber;
            FirstDriverResult = firstDriver;
            SecondDriverResult = secondDriver;
        }
    }
}
