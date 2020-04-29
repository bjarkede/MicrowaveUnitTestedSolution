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
    class IT4_DoorButtonLightDisplayUserInterface
    {
        private IUserInterface _utt;
        private IDoor _door;
        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDisplay _display;
        private ILight _light;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();

            _output = Substitute.For<IOutput>();

            _display = new Display(_output);
            _light = new Light(_output);

            _utt = new UserInterface(_powerButton,_timeButton,_startCancelButton, _door, _display, _light, null);
        }

        [Test]
        public void OnPowerPressed_Ready()
        {
            _powerButton.Press();
            _output.Received().OutputLine($"Display shows: {50} W");
        }

        [Test]
        public void OnPowerPressed_SetPower()
        {
            _powerButton.Press();
            _powerButton.Press();
            _output.Received().OutputLine($"Display shows: {100} W");
        }

        [Test]
        public void OnTimePressed_SetPower()
        {
            _powerButton.Press();
            _timeButton.Press();
            _output.Received().OutputLine($"Display shows: {1:D2}:{0:D2}");
        }

        [Test]
        public void OnTimePressed_SetTime()
        {
            _powerButton.Press();
            _timeButton.Press();
            _timeButton.Press();
            _output.Received().OutputLine($"Display shows: {2:D2}:{0:D2}");
        }

        [Test]
        public void OnStartCancelPressed_SetPower()
        {
            _powerButton.Press();
            _startCancelButton.Press();

            _output.Received().OutputLine("Display cleared");
        }


        [Test]
        public void OnDoorOpened_Ready()
        {
            _door.Open();
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void OnDoorOpened_SetPower()
        {
            _powerButton.Press();
            _door.Open();

            _output.Received().OutputLine("Display cleared");
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void OnDoorOpened_SetTime()
        {
            _powerButton.Press();
            _timeButton.Press();
            _door.Open();

            _output.Received().OutputLine("Display cleared");
            _output.Received().OutputLine("Light is turned on");
        }


        [Test]
        public void OnDoorClosed_DoorOpen()
        {
            _door.Open();
            _door.Close();

            _output.Received().OutputLine("Light is turned off");
        }

    }
}
