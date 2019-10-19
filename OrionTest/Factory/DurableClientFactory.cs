using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrionTest.Factory
{
    class DurableClientFactory
    {
        public static dynamic CreateDurableClient(string functionName)
        {
            var durableMock = new Mock<Mocks.DurableClient>();
            durableMock.Setup(x => x.StartNewAsync(functionName)).
                   ReturnsAsync(functionName);
            return durableMock.Object;
        }
    }
}
