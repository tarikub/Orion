using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Orion.Util;
using Orion.Factory;
using System.Collections.Generic;
using Xunit;
using OrionTest.Factory;

namespace Orion.Functions
{
    public class Orchestrator
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void RoutingTest()
        {
            var formCollection = new Dictionary<string, StringValues>
            {
                { "From", "111-555-4512" },
                {"Body", "" }
            };
            var request = TestFactory.CreateHttpRequest(formCollection);
            var durableClient = DurableClientFactory.CreateDurableClient("SomeFunction");
            var response = (OkObjectResult)await FnOrchestrator.Process(request, durableClient, logger);
            Assert.Equal(true, response.Value);
        }
    }
}
