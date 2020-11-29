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
            Assert.True(ctrl.childControls != null);
            Assert.True(ctrl.childControls.Count == 0);
        }
    
        [Fact]
        public void Test_ControlWithDataIsValid()
        {
            
            Control ctrl = new Control();
            ctrl.family = "AC";
            ctrl.number = "AC-1";
            ctrl.title = "ACCESS CONTROL";
            ctrl.priority = "P1";
            ctrl.lowimpact = true;
            ctrl.id = Guid.NewGuid();
            ChildControl cc = new ChildControl();
            cc.number = "AC-1.2.3.4.5(6)";
            cc.description = "My description is here";
            ctrl.childControls.Add(cc);

            // test things out
            Assert.True(ctrl != null);
            Assert.True(!string.IsNullOrEmpty(ctrl.family));
            Assert.True(!string.IsNullOrEmpty(ctrl.number));
            Assert.True(!string.IsNullOrEmpty(ctrl.title));
            Assert.True(!string.IsNullOrEmpty(ctrl.priority));
            Assert.True(ctrl.lowimpact);
            Assert.False(ctrl.moderateimpact);
            Assert.False(ctrl.highimpact);
            Assert.True(ctrl.childControls.Count == 1);
            Assert.True(ctrl.childControls[0].id != Guid.Empty);
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
