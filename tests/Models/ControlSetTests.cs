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
            ctrl.family = "AC";
            ctrl.number = "AC-1";
            ctrl.title = "ACCESS CONTROL";
            ctrl.priority = "P1";
            ctrl.lowimpact = true;
            ctrl.id = Guid.NewGuid();
            ctrl.subControlNumber = "AC-1 (b)";

            // test things out
            Assert.True(ctrl != null);
            Assert.True(!string.IsNullOrEmpty(ctrl.family));
            Assert.True(!string.IsNullOrEmpty(ctrl.number));
            Assert.True(!string.IsNullOrEmpty(ctrl.title));
            Assert.True(!string.IsNullOrEmpty(ctrl.priority));
            Assert.True(ctrl.lowimpact);
            Assert.False(ctrl.moderateimpact);
            Assert.False(ctrl.highimpact);
        }
    }
}
