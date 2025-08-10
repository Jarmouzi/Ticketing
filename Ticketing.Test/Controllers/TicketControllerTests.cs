using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using FluentAssertions;

namespace Ticketing.Test.Controllers
{
    public class TicketControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TicketControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTicket_ReturnsOk_WhenTicketExists()
        {
            var response = await _client.GetAsync("/api/tickets/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

}
