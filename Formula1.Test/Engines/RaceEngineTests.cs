using Formula1.Engines;
using Formula1.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Formula1.Test.Engines
{
    public class Tests
    {
        private const int PositionCombinationsPerRace = 311;

        private RaceEngine _raceEngine;

        [SetUp]
        public void Setup()
        {
            _raceEngine = new RaceEngine();
        }

        [Test]
        public void WhenRaceEngineInitialized_ThenResultsArePopulated()
        {
            var results = _raceEngine.PopulatePossibleRaceResults(1);

            Assert.IsNotEmpty(results);
        }

        [Test]
        public void WhenRaceEngineInitializedWithOneRace_ThenValidResultsAreReturned()
        {
            const int numRaces = 1;
            var results = _raceEngine.PopulatePossibleRaceResults(numRaces);

            Assert.AreEqual(PositionCombinationsPerRace, results.Count);
            AssertLastResultDNF(numRaces, results);
        }

        [Test]
        public void WhenRaceEngineInitializedWithTwoRaces_ThenValidResultsAreReturned()
        {
            const int numRaces = 2;
            var results = _raceEngine.PopulatePossibleRaceResults(numRaces);

            Assert.AreEqual(PositionCombinationsPerRace * numRaces, results.Count);
            AssertLastResultDNF(numRaces, results);
        }

        private void AssertLastResultDNF(int numberOfRaces, List<RaceResult> results)
        {
            for(int race = 1; race <= numberOfRaces; race++)
            {
                var result = results.LastOrDefault(p => p.RaceNumber == race);

                Assert.AreEqual(race, result.RaceNumber);
                Assert.AreEqual(Position.DNF, result.FirstDriverResult.Position);
                Assert.AreEqual(Position.DNF, result.SecondDriverResult.Position);
            }
        }
    }
}