using System.Collections.Generic;
using System.Linq;

namespace Formula1.Entities
{
    public class ChampionshipResult
    {
        public Driver Driver { get; set; }
        public List<Position> Positions { get; set; } = new();
        public int Points { get { return Positions.Sum(x => (int)x); } }
        public int NumberOfRacesTillWin { get { return Positions.Count; } }

        public ChampionshipResult(Driver driver)
        {
            Driver = driver;
        }
    }
}
