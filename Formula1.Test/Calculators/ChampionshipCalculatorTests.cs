using Formula1.Calculators;
using Formula1.Engines;
using Formula1.Entities;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula1.Test.Calculators
{
    public class ChampionshipCalculatorTests
    {
        private IRaceEngine _engine;
        private ChampionshipCalculator _calculator;

        private Driver _firstDriver;
        private Driver _secondDriver;

        private string Max_Verstappen = "Max Verstappen";
        private string Lewis_Hamilton = "Lewis Hamilton";

        [SetUp]
        public void Setup()
        {
            _engine = Substitute.For<IRaceEngine>();
            _calculator = new ChampionshipCalculator(_engine);

            _firstDriver = new Driver(Max_Verstappen);
            _secondDriver = new Driver(Lewis_Hamilton);
        }

        [Test]
        public void GivenACalculator_WhenCallingCalculate_ThenNoExceptionIsThrown()
        {
            _engine.PopulatePossibleRaceResults(Arg.Any<int>()).Returns(new List<RaceResult>());
            _calculator.Calculate(1, _firstDriver, _secondDriver);
        }

        [Test]
        public void GivenACalculator_WhenCallingCalculate_ThenEngineCalculatesPossiblePositionCombinations()
        {
            _engine.PopulatePossibleRaceResults(Arg.Any<int>()).Returns(new List<RaceResult>());
            _calculator.Calculate(1, _firstDriver, _secondDriver);
            _engine.Received().PopulatePossibleRaceResults(Arg.Any<int>());
        }

        [Test]
        public void GivenACalculator_WhenCallingCalculate_ThenWinningDriverIsReturned()
        {
            _engine.PopulatePossibleRaceResults(Arg.Any<int>()).Returns(
                new List<RaceResult>
                {
                    new RaceResult(1, new DriverResult(Position.P1), new DriverResult(Position.P2)),
                    new RaceResult(1, new DriverResult(Position.P2), new DriverResult(Position.P1)),
                    new RaceResult(2, new DriverResult(Position.P1), new DriverResult(Position.P2)),
                    new RaceResult(2, new DriverResult(Position.P2), new DriverResult(Position.P1))
                });

            var winner = _calculator.Calculate(2, _firstDriver, _secondDriver);
            Assert.IsNotNull(winner);
        }
    }
}
