using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Smtp;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Execution;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Unit
{
    class IT6_UserInterfaceCookController
    {
        private IUserInterface _utt;
        private CookController _cooker;
        private Door _door;
        private Button _powerButton;
        private Button _timeButton;
        private Button _startCancelButton;
        private Display _display;
        private PowerTube _powerTube;
        private Light _light;
        private IOutput _output;
        private Timer _timer;

        [SetUp]
        public void SetUp()
        {
            _timer = new Timer();
            _output = Substitute.For<IOutput>();

            _powerTube = new PowerTube(_output);
            _display = new Display(_output);
            _light = new Light(_output);
            _door = new Door();
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();

            _cooker = new CookController(_timer, _display, _powerTube);

            _utt = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cooker);

            _cooker.UI = _utt;
        }

        [Test]
        public void CookingIsDone_ReceivedCorrectly()
        {
            ManualResetEvent e = new ManualResetEvent(false);

            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _timer.Expired += (sender, args) => e.Set();

            e.WaitOne(); // Wait Untill the time has expired.

            // CookingIsDone() should now have been called from the _cooker.
            _output.Received().OutputLine($"Display cleared");
            _output.Received().OutputLine($"Light is turned off");
        }

        [TestCase(50)]
        public void OnStartCancelPressed_SetTime(int power)
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _output.Received().OutputLine("Light is turned on");
            _output.Received().OutputLine($"PowerTube works with {power}");
        }

        [Test]
        public void OnStartCancelPressed_Cooking()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            // The state is now cooking
            _startCancelButton.Press();

            _output.Received().OutputLine($"PowerTube turned off");
            _output.Received().OutputLine($"Display cleared");
            _output.Received().OutputLine("Light is turned off");
        }

        [Test]
        public void OnDoorOpened_Cooking()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _door.Open();

            _output.Received().OutputLine($"PowerTube turned off");
            _output.Received().OutputLine($"Display cleared");
        }
    }
}
