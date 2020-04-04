using Xunit;
using openrmf_msg_controls.Models;
using System;

namespace tests.Models
{
    public class ControlSetTests
    {
        [Fact]
        public void Test_NewControlIsValid()
        {
            ControlSet ctrl = new ControlSet();
            Assert.True(ctrl != null);
            Assert.True(ctrl.id != Guid.Empty);
        }
    
        [Fact]
        public void Test_ControlWithDataIsValid()
        {
            ControlSet ctrl = new ControlSet();
            ctrl.family = "AU";
            ctrl.number = "AU-9";
            ctrl.title = "My Title";
            ctrl.priority = "low";
            ctrl.lowimpact = false;
            ctrl.moderateimpact = true;
            ctrl.highimpact = false;
            ctrl.supplementalGuidance = "My guidance";
            ctrl.subControlDescription = "My sub description";
            ctrl.subControlNumber = "AU-9.1(b)";

            // test things out
            Assert.True(ctrl != null);
            Assert.True (!string.IsNullOrEmpty(ctrl.family));
            Assert.True (!string.IsNullOrEmpty(ctrl.number));
            Assert.True (!string.IsNullOrEmpty(ctrl.title));
            Assert.False (ctrl.lowimpact);
            Assert.True (ctrl.moderateimpact);
            Assert.False (ctrl.highimpact);
            Assert.True (!string.IsNullOrEmpty(ctrl.supplementalGuidance));
            Assert.True (!string.IsNullOrEmpty(ctrl.subControlDescription));
            Assert.True (!string.IsNullOrEmpty(ctrl.subControlNumber));
            Assert.True (ctrl.id != Guid.Empty);
        }
    }
}
