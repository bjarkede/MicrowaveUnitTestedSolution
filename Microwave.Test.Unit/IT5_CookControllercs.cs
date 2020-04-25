using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Smtp;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;


namespace Microwave.Test.Unit
{
    [TestFixture]
    class IT5_CookControllercs
    {
        private ICookController _utt;
        private IPowerTube _powerTube;
        private IDisplay _display;
        private IOutput _output;
        private ITimer _timer;

        [SetUp]
        public void SetUp()
        {
            // Stub
            _timer = Substitute.For<ITimer>();
            _output = Substitute.For<IOutput>();

            _powerTube = new PowerTube(_output);
            _display = new Display(_output);

            _utt = new CookController(_timer, _display, _powerTube);
        }

        [TestCase(50, 60)]
        public void StartCooking_IsAnnouncedCorrectly(int power, int time)
        {
            _utt.StartCooking(power, time);

            _output.Received().OutputLine($"PowerTube works with {power}");
            _timer.Received().Start(time);
        }

    }
}
