using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using openrmf_msg_controls.Classes;
using openrmf_msg_controls.Database;
using openrmf_msg_controls.Models;
using Xunit;

namespace tests.Classes;

[CollectionDefinition("ControlsLoaderFileCollection", DisableParallelization = true)]
public class ControlsLoaderFileCollectionDefinition;

[Collection("ControlsLoaderFileCollection")]
public class ControlsLoaderTests
{
    private static readonly string ControlsXmlPath = Path.Combine(AppContext.BaseDirectory, "800-53-controls.xml");

    [Fact]
    public void LoadControls_WhenXmlExists_ParsesControlsAndChildren()
    {
        var xml = BuildSampleXml();
        using var fileScope = new ControlsXmlFileScope(xml);

        var controls = ControlsLoader.LoadControls();

        Assert.NotNull(controls);
        Assert.Single(controls);

        var control = controls[0];
        Assert.Equal("AC", control.family);
        Assert.Equal("AC-1", control.number);
        Assert.Equal("Access Control", control.title);
        Assert.Equal("P1", control.priority);
        Assert.True(control.lowimpact);
        Assert.Contains(control.childControls, x => x.number == "AC-1 (1)." && x.lowimpact);
        Assert.Contains(control.childControls, x => x.number == "AC-1 (2)." && x.highimpact);
        Assert.Equal("Guidance text", control.supplementalGuidance);
    }

    [Fact]
    public void LoadControlsXML_WhenXmlExists_PersistsFlattenedRows()
    {
        var xml = BuildSampleXml();
        using var fileScope = new ControlsXmlFileScope(xml);

        var options = new DbContextOptionsBuilder<ControlsDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new ControlsDBContext(options);
        ControlsLoader.LoadControlsXML(context);

        var items = context.ControlSets.OrderBy(x => x.subControlNumber).ToList();
        Assert.Equal(2, items.Count);
        Assert.Contains(items, x => x.subControlNumber == "AC-1(1)" && x.lowimpact);
        Assert.Contains(items, x => x.subControlNumber == "AC-1(2)" && x.highimpact);
        Assert.All(items, x => Assert.Equal("AC", x.family));
    }

    [Fact]
    public void LoadControlsXML_UsesContextMethods_MoqVerification()
    {
        var xml = BuildSampleXml();
        using var fileScope = new ControlsXmlFileScope(xml);

        var options = new DbContextOptionsBuilder<ControlsDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var mockDbSet = new Mock<DbSet<ControlSet>>();
        var mockContext = new Mock<ControlsDBContext>(options) { CallBase = true };
        mockContext.Setup(x => x.ControlSets).Returns(mockDbSet.Object);
        mockContext.Setup(x => x.SaveChanges()).Returns(1);

        ControlsLoader.LoadControlsXML(mockContext.Object);

        mockDbSet.Verify(x => x.Add(It.IsAny<ControlSet>()), Times.AtLeastOnce);
        mockContext.Verify(x => x.SaveChanges(), Times.AtLeastOnce);
    }

    [Fact]
    public void LoadControls_WhenXmlMissing_ReturnsEmptyList()
    {
        using var fileScope = new ControlsXmlFileScope(deleteFile: true);

        var controls = ControlsLoader.LoadControls();

        Assert.NotNull(controls);
        Assert.Empty(controls);
    }

    private static string BuildSampleXml()
    {
        return """
<root xmlns:controls="urn:controls">
  <controls:control>
    <family>AC</family>
    <number>AC-1</number>
    <title>Access Control</title>
    <priority>P1</priority>
    <baseline-impact>LOW</baseline-impact>
    <statement>
      <statement>
        <number>AC-1 (1).</number>
        <description>Low child control</description>
      </statement>
    </statement>
    <control-enhancements>
      <control-enhancement>
        <number>AC-1 (2).</number>
        <description>High child control</description>
        <baseline-impact>HIGH</baseline-impact>
      </control-enhancement>
    </control-enhancements>
    <supplemental-guidance>
      <p>Guidance text</p>
    </supplemental-guidance>
  </controls:control>
</root>
""";
    }

    private sealed class ControlsXmlFileScope : IDisposable
    {
        private readonly bool _originalExisted;
        private readonly string _originalContent;

        public ControlsXmlFileScope(string content)
        {
            _originalExisted = File.Exists(ControlsXmlPath);
            _originalContent = _originalExisted ? File.ReadAllText(ControlsXmlPath) : string.Empty;

            File.WriteAllText(ControlsXmlPath, content);
        }

        public ControlsXmlFileScope(bool deleteFile)
        {
            _originalExisted = File.Exists(ControlsXmlPath);
            _originalContent = _originalExisted ? File.ReadAllText(ControlsXmlPath) : string.Empty;

            if (deleteFile && File.Exists(ControlsXmlPath))
            {
                File.Delete(ControlsXmlPath);
            }
        }

        public void Dispose()
        {
            if (_originalExisted)
            {
                File.WriteAllText(ControlsXmlPath, _originalContent);
            }
            else if (File.Exists(ControlsXmlPath))
            {
                File.Delete(ControlsXmlPath);
            }
        }
    }
}