using Xunit;
using openrmf_msg_controls.Models;
using System;

namespace tests.Models
{
    public class FilterTests
    {
        [Fact]
        public void Test_NewFilterIsValid()
        {
            Filter filter = new Filter();
            Assert.True(filter != null);
            Assert.True(filter.impactLevel == "low");
            Assert.False(filter.pii);
        }
    
        [Fact]
        public void Test_FilterWithDataIsValid()
        {
            Filter filter = new Filter();
            filter.impactLevel = "high";
            filter.pii = true;

            // test things out
            Assert.True(filter != null);
            Assert.True (!string.IsNullOrEmpty(filter.impactLevel));
            Assert.True (filter.pii);
        }

    }
}
