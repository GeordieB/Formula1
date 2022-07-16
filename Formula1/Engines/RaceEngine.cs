using Formula1.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Formula1.Engines
{
    public interface IRaceEngine
    {
        List<RaceResult> PopulatePossibleRaceResults(int numberOfRaces);
    }

    public class RaceEngine : IRaceEngine
    {
        private int _numberOfRaces;
        private List<RaceResult> _results = new();
        private Position[] _positions = new Position[]{
            Position.P1, Position.P2, Position.P3,
            Position.P4, Position.P5, Position.P6,
            Position.P7, Position.P8, Position.P9,
            Position.P10, Position.DNF
        };


        public List<RaceResult> PopulatePossibleRaceResults(int numberOfRaces)
        {
            _numberOfRaces = numberOfRaces;
            InitializeRaceResults();

            return _results;
        }

        private void InitializeRaceResults()
        {
            _results.Clear();
            for (int race = 1; race <= _numberOfRaces; race++)
            {
                AddAllPositions(race);
                AddFastestLap(race);
                AddDoubleDNF(race);
            }
        }

        private void AddAllPositions(int raceNumber)
        {
            _results.AddRange(_positions.SelectMany(x => _positions.Select(p => new RaceResult(raceNumber, new DriverResult(x), new DriverResult(p))))
                       .Where(x => x.FirstDriverResult.Position != x.SecondDriverResult.Position)
                       .ToList());
        }

        private void AddFastestLap(int raceNumber)
        {
            var temp = new List<RaceResult>();
            foreach (var result in _results.Where(p => p.RaceNumber == raceNumber))
            {
                if(result.FirstDriverResult.Position != Position.DNF)
                    temp.Add(new RaceResult(raceNumber, new DriverResult(result.FirstDriverResult.Position, true), new DriverResult(result.SecondDriverResult.Position)));

                if (result.SecondDriverResult.Position != Position.DNF)
                    temp.Add(new RaceResult(raceNumber, new DriverResult(result.FirstDriverResult.Position), new DriverResult(result.SecondDriverResult.Position, true)));
            }
            _results.AddRange(temp);
        }

        private void AddDoubleDNF(int raceNumber)
        {
            _results.Add(new RaceResult(raceNumber, new DriverResult(Position.DNF), new DriverResult(Position.DNF)));
        }
    }
}
