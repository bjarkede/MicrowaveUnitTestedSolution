using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Microwave.Test.Unit
{
    [TestFixture]
    class IT2_OutputDisplay
    {
        private IDisplay _utt;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _utt = new Display(_output);
        }

        [TestCase(5, 30)]
        public void ShowTime_DisplayIsAnnouncedCorrectly(int min, int sec)
        {
            _utt.ShowTime(min, sec);
            _output.Received().OutputLine($"Display shows: {min:D2}:{sec:D2}");
        }

        [TestCase(50)]
        public void ShowPower_DisplayIsAnnouncedCorrectly(int power)
        {
            _utt.ShowPower(power);
            _output.Received().OutputLine($"Display shows: {power} W");
        }

        [Test]
        public void Clear_DisplayIsAnnouncedCorrectly()
        {
            _utt.Clear();
            _output.Received().OutputLine("Display cleared");
        }
    }
}
