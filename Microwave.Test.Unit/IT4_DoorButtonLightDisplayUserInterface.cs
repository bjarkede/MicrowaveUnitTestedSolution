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
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();

            _door = Substitute.For<IDoor>();
            _display = Substitute.For<IDisplay>();
            _light = Substitute.For<ILight>();

            _cooker = Substitute.For<ICookController>(); // Not part of this integration step

            _utt = new UserInterface(_powerButton,_timeButton,_startCancelButton, _door, _display, _light, _cooker);
        }

        [Test]
        public void OnPowerPressed_Ready()
        {
            _utt.OnPowerPressed(null, null);
            _display.Received().ShowPower(50);
        }

        [Test]
        public void OnPowerPressed_SetPower()
        {
            _utt.OnPowerPressed(null, null);
            _utt.OnPowerPressed(null, null); // This triggers the States.SETPOWER case
            _display.Received().ShowPower(50 + 50);
        }

        [Test]
        public void OnTimePressed_SetPower()
        {
            _utt.OnPowerPressed(null, null);
            _utt.OnTimePressed(null,null);
            _display.Received().ShowTime(1,0);
        }

        [Test]
        public void OnTimePressed_SetTime()
        {
            _utt.OnPowerPressed(null, null); // Make sure our state is States.SETPOWER
            _utt.OnTimePressed(null, null); 
            _utt.OnTimePressed(null, null); // This reaches the States.SETTIME case
            _display.Received().ShowTime(2, 0);
        }

        [Test]
        public void OnStartCancelPressed_SetPower()
        {
            _utt.OnPowerPressed(null, null);
            _utt.OnStartCancelPressed(null , null);

            _light.Received().TurnOff();
            _display.Received().Clear();
        }

        [TestCase(50, 1)]
        public void OnStartCancelPressed_SetTime(int powerLevel, int time)
        {
            _utt.OnPowerPressed(null, null);
            _utt.OnTimePressed(null,null);
            _utt.OnStartCancelPressed(null, null);

            _light.Received().TurnOn();
            _cooker.ReceivedWithAnyArgs().StartCooking(powerLevel, time*60);
        }

        [Test]
        public void OnStartCancelPressed_Cooking()
        {
            _utt.OnPowerPressed(null, null);
            _utt.OnTimePressed(null, null);
            _utt.OnStartCancelPressed(null, null);

            // The state is now cooking
            _utt.OnStartCancelPressed(null, null);

            _cooker.Received().Stop();
            _light.Received().TurnOff();
            _display.Received().Clear();
        }

        [Test]
        public void OnDoorOpened_Ready()
        {

        }

        [Test]
        public void OnDoorOpened_SetPower()
        {

        }

        [Test]
        public void OnDoorOpened_SetTime()
        {

        }

        [Test]
        public void OnDoorOpened_Cooking()
        {

        }

        [Test]
        public void OnDoorClosed_DoorOpen()
        {

        }

        [Test]
        public void CookingIsDone_Cooking()
        {

        }
    }
}
