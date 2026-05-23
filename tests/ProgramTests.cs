using System.Reflection;
using openrmf_msg_controls.Classes;
using Xunit;

namespace tests;

public class ProgramTests
{
    [Theory]
    [InlineData("AC 1", 2)]
    [InlineData("AC.1", 2)]
    [InlineData("AC 1.2", 2)]
    [InlineData("AC-1", -1)]
    [InlineData("", -1)]
    public void GetFirstIndex_ReturnsExpectedIndex(string input, int expected)
    {
        var actual = InvokeGetFirstIndex(input);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetFirstIndex_PrioritizesFirstDelimiter()
    {
        var input = "AC.1 2";

        var actual = InvokeGetFirstIndex(input);

        Assert.Equal(2, actual);
        Assert.NotEqual(4, actual);
    }

    private static int InvokeGetFirstIndex(string input)
    {
        var assembly = typeof(ControlsLoader).Assembly;
        var programType = assembly.GetType("openrmf_msg_controls.Program", throwOnError: true);
        var method = programType!.GetMethod("GetFirstIndex", BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(method);

        return (int)method!.Invoke(null, [input])!;
    }
}