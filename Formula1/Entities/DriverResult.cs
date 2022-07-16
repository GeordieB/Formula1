namespace Formula1.Entities
{
    public class DriverResult
    {
        public Position Position { get; set; }
        public bool FastestLap { get; set; }
        public int Points { get { return FastestLap ? (int)Position + Constants.FastestLap : (int)Position; } }

        public DriverResult(Position position, bool fastestLap = false)
        {
            Position = position;
            FastestLap = fastestLap;
        }
    }
}
