using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Unit
{
    [TestFixture]
    class IT1_OutputPowerTube
    {
        private IOutput _output;
        private PowerTube _utt;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _utt = new PowerTube(_output);
        }

        [Test]
        public void TurnOff_PowerTubeIsAnnouncedCorrectly()
        {
            _utt.TurnOn(50);
            _utt.TurnOff();

            _output.Received().OutputLine("PowerTube turned off");
        }

        [Test]
        public void TurnOn_PowerTubeIsAnnouncedCorrectly()
        {
            _utt.TurnOn(50);
            _output.Received().OutputLine("PowerTube works with 50");
        }
    }
}
