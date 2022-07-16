namespace Formula1.Entities
{
    public class Driver
    {
        public string Name { get; set; }
        public int InitialPoints { get; set; }

        public Driver(string name, int initialPoints = 0)
        {
            Name = name;
            InitialPoints = initialPoints;
        }
    }
}
