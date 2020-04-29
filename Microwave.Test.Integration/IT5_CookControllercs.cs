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
using ITimer = MicrowaveOvenClasses.Interfaces.ITimer;

namespace Microwave.Test.Unit
{
    [TestFixture]
    class IT5_CookControllercs
    {
        private CookController _utt;
        private PowerTube _powerTube;
        private Display _display;
        private IOutput _output;
        private ITimer _timer;

        [SetUp]
        public void SetUp()
        {
            // Stub
            _timer = Substitute.For<Timer>();
            _output = Substitute.For<IOutput>();

            _powerTube = new PowerTube(_output);
            _display = new Display(_output);

            _utt = new CookController(_timer, _display, _powerTube);
        }

        #region PowerTube

        [TestCase(50, 60)]
        public void StartCooking_IsAnnouncedCorrectly(int power, int time)
        {
            _utt.StartCooking(power, time);

            _output.Received().OutputLine($"PowerTube works with {power}");
            _timer.Received().Start(time);
        }

        [TestCase(101)]
        [TestCase(0)]
        public void StartCooking_ArgumentOutOfRange(int power)
        {
            Assert.That(() => _utt.StartCooking(power, 60), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void StartCooking_ApplicatedException()
        {
            _utt.StartCooking(50, 50);
            Assert.That(() => _utt.StartCooking(50, 50), Throws.TypeOf<ApplicationException>());
        }

        [TestCase(50,60)]
        public void Stop_IsAnnouncedCorrectly(int power, int time)
        {
            _utt.StartCooking(power, time);
            _utt.Stop();
            _output.Received().OutputLine($"PowerTube turned off");
        }

        [Test]
        public void StopException_IsAnnouncedCorrectly()
        {
            ManualResetEvent e = new ManualResetEvent(false);
            _timer.Expired += (sender, args) => e.Set();

            _utt.StartCooking(50, 1);

            e.WaitOne(1100);
            
            _output.Received().OutputLine($"PowerTube turned off");
        }

        #endregion

        #region Display

        // We already kind of tested this, when doing the .ShowTime() functions in #Timer

        #endregion

        #region Timer

        [TestCase(50, 60)]
        public void StartCooking_ShowTimeIsAnnouncedCorrectly(int power, int time)
        {
            ManualResetEvent e = new ManualResetEvent(false);
            _timer.TimerTick += (sender, args) => e.Set();

            _utt.StartCooking(power, time);

            e.WaitOne(1500); // Wait until a tick happens.

            _output.Received().OutputLine($"Display shows: {_timer.TimeRemaining/60:D2}:{_timer.TimeRemaining % 60:D2}");
        }

        [TestCase(50, 60)]
        public void StartCooking_ShowTimeIsNotReceivedYet(int power, int time)
        {
            _utt.StartCooking(power, time);
            _output.DidNotReceive().OutputLine($"Display shows: {_timer.TimeRemaining / 60:D2}:{_timer.TimeRemaining % 60:D2}");
        }

        [TestCase(50, 60)]
        [TestCase(50, 100)]
        [TestCase(50, 0)]
        [TestCase(50, -100)]
        public void StartCooking_TimeIsSetCorrectly(int power, int time)
        {
            _utt.StartCooking(power, time);
            Assert.That(_timer.TimeRemaining == time);
        }

        #endregion

    }
}
