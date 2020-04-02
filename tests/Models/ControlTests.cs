using Xunit;
using openrmf_msg_controls.Models;
using System;

namespace tests.Models
{
    public class ControlTests
    {
        [Fact]
        public void Test_NewControlIsValid()
        {
            Control ctrl = new Control();
            Assert.True(ctrl != null);
        }
    
        [Fact]
        public void Test_ControlWithDataIsValid()
        {
            Control ctrl = new Control();
            ctrl.family = "AU-9";
            ctrl.number = "AU-9";
            ctrl.title = "Audit Control";
            ctrl.lowimpact = true;
            ctrl.moderateimpact = true;
            ctrl.highimpact = true;
            ctrl.supplementalGuidance = "Do this once and forever";
            ctrl.id = Guid.NewGuid();

            // test things out
            Assert.True(ctrl != null);
            Assert.True (!string.IsNullOrEmpty(ctrl.family));
            Assert.True (!string.IsNullOrEmpty(ctrl.number));
            Assert.True (!string.IsNullOrEmpty(ctrl.title));
            Assert.True (ctrl.lowimpact);
            Assert.True (ctrl.moderateimpact);
            Assert.True (ctrl.highimpact);
            Assert.True (!string.IsNullOrEmpty(ctrl.supplementalGuidance));
            Assert.True (ctrl.id != Guid.Empty);
            Assert.True (ctrl.childControls != null);
            Assert.True (ctrl.childControls.Count == 0);
        }

        [Fact]
        public void Test_NewChildControlIsValid()
        {
            ChildControl ctrl = new ChildControl();
            Assert.True(ctrl != null);
        }

        [Fact]
        public void Test_ChildControlWithDataIsValid()
        {
            ChildControl ctrl = new ChildControl();
            ctrl.description = "AU-9";
            ctrl.number = "AU-9";

            // test things out
            Assert.True(ctrl != null);
            Assert.True (!string.IsNullOrEmpty(ctrl.description));
            Assert.True (!string.IsNullOrEmpty(ctrl.number));
            Assert.True (ctrl.id != Guid.Empty);
        }

    }
}
