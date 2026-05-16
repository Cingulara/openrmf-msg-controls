using System;
using openrmf_msg_controls.Classes;
using Xunit;

namespace tests.Classes;

public class CompressionTests
{
    [Fact]
    public void CompressAndDecompress_RoundTripsOriginalText()
    {
        const string input = "Control AC-1 with child control AC-1(1)";

        var compressed = Compression.CompressString(input);
        var decompressed = Compression.DecompressString(compressed);

        Assert.False(string.IsNullOrWhiteSpace(compressed));
        Assert.Equal(input, decompressed);
    }

    [Fact]
    public void CompressAndDecompress_RoundTripsEmptyText()
    {
        const string input = "";

        var compressed = Compression.CompressString(input);
        var decompressed = Compression.DecompressString(compressed);

        Assert.NotNull(compressed);
        Assert.Equal(input, decompressed);
    }

    [Fact]
    public void DecompressString_WithInvalidBase64_ThrowsFormatException()
    {
        const string invalidBase64 = "not-base64";

        Assert.Throws<FormatException>(() => Compression.DecompressString(invalidBase64));
    }
}