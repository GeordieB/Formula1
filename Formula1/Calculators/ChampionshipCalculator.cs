using Formula1.Engines;
using Formula1.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Formula1.Calculators
{
    public interface IChampionshipCalculator
    {
        Driver Calculate(int numberOfRacesLeft, Driver firstDriver, Driver secondDriver);
    }

    public class ChampionshipCalculator : IChampionshipCalculator
    {
        private IRaceEngine _engine;
        private int _numberOfRacesLeft;
        private List<RaceResult> _possibleRaceResults;
        private Dictionary<int, List<RaceResult>> _raceResultDictionary;
        private Driver _firstDriver;
        private Driver _secondDriver;
        private int _iterations;

        public ChampionshipCalculator(IRaceEngine raceEngine)
        {
            _engine = raceEngine;
        }

        public Driver Calculate(int numberOfRacesLeft, Driver firstDriver, Driver secondDriver)
        {
            _numberOfRacesLeft = numberOfRacesLeft;
            _firstDriver = firstDriver;
            _secondDriver = secondDriver;

            Init();

            _raceResultDictionary = ToDictionary();
            _iterations = _raceResultDictionary.FirstOrDefault().Value.Count;
            var combinations = new List<List<RaceResult>>();
            var innerCombinations = new List<RaceResult>();
            var temp = _raceResultDictionary;
            CombineRaceResults(temp, combinations, innerCombinations);

            List<ChampionshipResult> driver1ChampionshipResults = new();
            List<ChampionshipResult> driver2ChampionshipResults = new();

            //TODO: Add logic
            foreach (var raceCombination in combinations)
            {
                var driver1 = new ChampionshipResult(firstDriver);
                var driver2 = new ChampionshipResult(secondDriver);
                var racesLeft = _numberOfRacesLeft;

                foreach (var raceResult in raceCombination)
                {
                    driver1.Positions.Add(raceResult.FirstDriverResult.Position);
                    driver2.Positions.Add(raceResult.SecondDriverResult.Position);
                    racesLeft--;
                    if (IsWinner(driver1.Points, driver2.Points, racesLeft) || IsWinner(driver2.Points, driver1.Points, racesLeft))
                        break;
                }
                driver1ChampionshipResults.Add(driver1);
                driver2ChampionshipResults.Add(driver2);
            }
            return _firstDriver;
        }

        private RaceResult CombineRaceResults(Dictionary<int, List<RaceResult>> raceResults,
            List<List<RaceResult>> combinations, List<RaceResult> innerCombinations,
            int iteration = 0)
        {
            if (raceResults.Count == 1)
            {
                if (iteration == _iterations + 1)
                    return null;
                return raceResults.FirstOrDefault().Value.ElementAt(iteration);
            }

            var temp = Copy(raceResults);
            temp.Remove(raceResults.FirstOrDefault().Key);

            foreach (var raceOutcome in raceResults.FirstOrDefault().Value)
            {
                for (int i = 0; i < _iterations; i++)
                {
                    innerCombinations.Add(raceOutcome);
                    innerCombinations.Add(CombineRaceResults(temp, combinations, innerCombinations, i));

                    if (raceResults.Count == _numberOfRacesLeft)
                    {
                        combinations.Add(innerCombinations);
                        innerCombinations = new();
                    }
                }
            }

            return null;
        }

        private Dictionary<int, List<RaceResult>> ToDictionary()
        {
            Dictionary<int, List<RaceResult>> races = new();
            foreach (var raceResult in _possibleRaceResults)
            {
                if (!races.TryAdd(raceResult.RaceNumber, new List<RaceResult> { raceResult }))
                    races[raceResult.RaceNumber].Add(raceResult);
            }

            return races;
        }

        private Dictionary<int, List<RaceResult>> Copy(Dictionary<int, List<RaceResult>> dictionary)
        {
            var temp = new Dictionary<int, List<RaceResult>>();
            foreach (var entry in dictionary)
            {
                temp.TryAdd(entry.Key, entry.Value);
            }

            return temp;
        }

        //private ChampionshipResult GetChampionshipResult()
        //{
        //    return DriverWins(_firstDriver, _secondDriver) ? new ChampionshipResult(_firstDriver) :
        //            new ChampionshipResult(_secondDriver);
        //}

        private void Init()
        {
            _possibleRaceResults = _engine.PopulatePossibleRaceResults(_numberOfRacesLeft);
        }

        private bool IsWinner(int driver1Points, int driver2Points, int numberOfRacesLeft)
        {
            return driver2Points + GetMaxPointsAvailable(numberOfRacesLeft) < driver1Points;
        }

        private int GetMaxPointsAvailable(int numberOfRacesLeft)
        {
            return numberOfRacesLeft * (int)Position.P1 + Constants.FastestLap;
        }
    }
}
