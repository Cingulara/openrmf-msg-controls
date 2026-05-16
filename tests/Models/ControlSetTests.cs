using System;
using openrmf_msg_controls.Models;
using Xunit;

namespace tests.Models;

public class ControlSetTests
{
    [Fact]
    public void Constructor_InitializesId()
    {
        var controlSet = new ControlSet();

        Assert.NotNull(controlSet);
        Assert.NotEqual(Guid.Empty, controlSet.id);
    }

    [Fact]
    public void Properties_StoreAssignedValues()
    {
        var id = Guid.NewGuid();
        var controlSet = new ControlSet
        {
            id = id,
            family = "AC",
            number = "AC-1",
            title = "ACCESS CONTROL",
            priority = "P1",
            lowimpact = true,
            moderateimpact = false,
            highimpact = false,
            supplementalGuidance = "Guidance",
            subControlDescription = "Sub description",
            subControlNumber = "AC-1(b)"
        };

        Assert.Equal(id, controlSet.id);
        Assert.Equal("AC", controlSet.family);
        Assert.Equal("AC-1", controlSet.number);
        Assert.Equal("ACCESS CONTROL", controlSet.title);
        Assert.Equal("P1", controlSet.priority);
        Assert.True(controlSet.lowimpact);
        Assert.False(controlSet.moderateimpact);
        Assert.False(controlSet.highimpact);
        Assert.Equal("Guidance", controlSet.supplementalGuidance);
        Assert.Equal("Sub description", controlSet.subControlDescription);
        Assert.Equal("AC-1(b)", controlSet.subControlNumber);
    }
}
