using System;
using openrmf_msg_controls.Models;
using Xunit;

namespace tests.Models;

public class ControlTests
{
    [Fact]
    public void Constructor_InitializesChildControlsAndId()
    {
        var control = new Control();

        Assert.NotNull(control);
        Assert.NotNull(control.childControls);
        Assert.Empty(control.childControls);
        Assert.NotEqual(Guid.Empty, control.id);
    }

    [Fact]
    public void Properties_StoreAssignedValues()
    {
        var id = Guid.NewGuid();
        var child = new ChildControl
        {
            number = "AC-1 (1)",
            description = "Sub control"
        };

        var control = new Control
        {
            family = "AC",
            number = "AC-1",
            title = "ACCESS CONTROL",
            priority = "P1",
            lowimpact = true,
            moderateimpact = false,
            highimpact = false,
            supplementalGuidance = "Guidance",
            id = id
        };

        control.childControls.Add(child);

        Assert.Equal("AC", control.family);
        Assert.Equal("AC-1", control.number);
        Assert.Equal("ACCESS CONTROL", control.title);
        Assert.Equal("P1", control.priority);
        Assert.True(control.lowimpact);
        Assert.False(control.moderateimpact);
        Assert.False(control.highimpact);
        Assert.Equal("Guidance", control.supplementalGuidance);
        Assert.Equal(id, control.id);
        Assert.Single(control.childControls);
        Assert.NotEqual(Guid.Empty, control.childControls[0].id);
    }

    [Fact]
    public void ChildControl_ConstructorInitializesId()
    {
        var childControl = new ChildControl();

        Assert.NotNull(childControl);
        Assert.NotEqual(Guid.Empty, childControl.id);
    }

    [Fact]
    public void ChildControl_PropertiesStoreAssignedValues()
    {
        var childControl = new ChildControl
        {
            description = "Audit events",
            number = "AU-2",
            lowimpact = true,
            moderateimpact = false,
            highimpact = true
        };

        Assert.Equal("Audit events", childControl.description);
        Assert.Equal("AU-2", childControl.number);
        Assert.True(childControl.lowimpact);
        Assert.False(childControl.moderateimpact);
        Assert.True(childControl.highimpact);
        Assert.NotEqual(Guid.Empty, childControl.id);
    }
}
