using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using openrmf_msg_controls.Database;
using openrmf_msg_controls.Models;
using Xunit;

namespace tests.Database;

public class ControlsDBContextTests
{
    [Fact]
    public void ControlSets_CanPersistAndReadRecords()
    {
        var options = new DbContextOptionsBuilder<ControlsDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new ControlsDBContext(options);
        var item = new ControlSet
        {
            family = "AC",
            number = "AC-1",
            title = "Access Control",
            priority = "P1",
            lowimpact = true,
            subControlNumber = "AC-1(1)",
            subControlDescription = "Sub-control"
        };

        context.ControlSets.Add(item);
        context.SaveChanges();

        var saved = context.ControlSets.Single();
        Assert.Equal("AC", saved.family);
        Assert.Equal("AC-1", saved.number);
        Assert.Equal("AC-1(1)", saved.subControlNumber);
        Assert.True(saved.lowimpact);
        Assert.False(saved.highimpact);
    }

    [Fact]
    public void ControlSets_IsAvailableOnContext()
    {
        var options = new DbContextOptionsBuilder<ControlsDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new ControlsDBContext(options);

        Assert.NotNull(context.ControlSets);
    }
}