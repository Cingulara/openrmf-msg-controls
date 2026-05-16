using openrmf_msg_controls.Models;
using Xunit;

namespace tests.Models;

public class FilterTests
{
    [Fact]
    public void Constructor_SetsDefaultImpactLevelAndPii()
    {
        var filter = new Filter();

        Assert.NotNull(filter);
        Assert.Equal("low", filter.impactLevel);
        Assert.False(filter.pii);
        Assert.NotEqual("high", filter.impactLevel);
    }

    [Theory]
    [InlineData("low", false)]
    [InlineData("moderate", false)]
    [InlineData("high", true)]
    [InlineData("custom", true)]
    public void Properties_CanBeAssignedAndReadBack(string impactLevel, bool pii)
    {
        var filter = new Filter
        {
            impactLevel = impactLevel,
            pii = pii
        };

        Assert.Equal(impactLevel, filter.impactLevel);
        Assert.Equal(pii, filter.pii);
    }
}
