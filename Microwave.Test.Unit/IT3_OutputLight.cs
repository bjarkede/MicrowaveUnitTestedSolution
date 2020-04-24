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
    class IT3_OutputLight
    {
        private ILight _utt;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _utt = new Light(_output);
        }

        [Test]
        public void TurnOn_DisplayIsAnnouncedCorrectly()
        {
            _utt.TurnOn();
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void TurnOff_DisplayIsAnnouncedCorrectly()
        {
            _utt.TurnOn(); 
            _utt.TurnOff(); // Assumes that the light was turned on.
            _output.Received().OutputLine("Light is turned off");
        }

    }
}
