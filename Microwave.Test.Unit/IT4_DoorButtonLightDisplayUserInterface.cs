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

        private ICookController _cooker;

        [SetUp]
        public void SetUp()
        {
            _powerButton = Substitute.For<Button>();
            _timeButton = Substitute.For<Button>();
            _startCancelButton = Substitute.For<Button>();

            _door = Substitute.For<Door>();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();

            _cooker = Substitute.For<ICookController>(); // Not part of this integration step

            _utt = new UserInterface(_powerButton,_timeButton,_startCancelButton, _door, _display, _light, _cooker);
        }

        [Test]
        public void OnPowerPressed_Ready()
        {
            _powerButton.Press();
            _display.Received().ShowPower(50);
        }

        [Test]
        public void OnPowerPressed_SetPower()
        {
            _powerButton.Press();
            _powerButton.Press();
            _display.Received().ShowPower(50 + 50);
        }

        [Test]
        public void OnTimePressed_SetPower()
        {
            _powerButton.Press();
            _timeButton.Press();
            _display.Received().ShowTime(1,0);
        }

        [Test]
        public void OnTimePressed_SetTime()
        {
            _powerButton.Press();
            _timeButton.Press();
            _timeButton.Press();
            _display.Received().ShowTime(2, 0);
        }

        [Test]
        public void OnStartCancelPressed_SetPower()
        {
            _powerButton.Press();
            _startCancelButton.Press();

            _light.Received().TurnOff();
            _display.Received().Clear();
        }

        [TestCase(50, 1)]
        public void OnStartCancelPressed_SetTime(int powerLevel, int time)
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            _light.Received().TurnOn();
            _cooker.ReceivedWithAnyArgs().StartCooking(powerLevel, time*60);
        }

        [Test]
        public void OnStartCancelPressed_Cooking()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            // The state is now cooking
            _startCancelButton.Press();

            _cooker.Received().Stop();
            _light.Received().TurnOff();
            _display.Received().Clear();
        }

        [Test]
        public void OnDoorOpened_Ready()
        {
            _door.Open();
            _light.Received().TurnOn();
        }

        [Test]
        public void OnDoorOpened_SetPower()
        {
            _powerButton.Press();
            _door.Open();

            _light.Received().TurnOn();
            _display.Received().Clear();
        }

        [Test]
        public void OnDoorOpened_SetTime()
        {
            _powerButton.Press();
            _timeButton.Press();
            _door.Open();

            _light.Received().TurnOn();
            _display.Received().Clear();
        }

        [Test]
        public void OnDoorOpened_Cooking()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();
            _door.Open();

            _cooker.Received().Stop();
            _display.Received().Clear();
        }

        [Test]
        public void OnDoorClosed_DoorOpen()
        {
            _door.Open();
            _door.Close();

            _light.Received().TurnOff();
        }

        [Test]
        public void CookingIsDone_Cooking()
        {
            _powerButton.Press();
            _timeButton.Press();
            _startCancelButton.Press();

            // @TODO
        }
    }
}
