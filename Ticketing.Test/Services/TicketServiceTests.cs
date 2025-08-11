using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Ticketing.Application.Services;
using Ticketing.Common.Results;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.interfaces;
using Ticketing.Repository;
using Ticketing.Test.TestData;
using FluentAssertions;

namespace Ticketing.Test;

public class TicketServiceTests
{
    private readonly Mock<ITicketRepository> _ticketRepositoryMock;
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<TicketService>> _LoggerMock;

    private readonly TicketService _service;

    public TicketServiceTests()
    {
        _ticketRepositoryMock = new Mock<ITicketRepository>();
        _authRepositoryMock = new Mock<IAuthRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _LoggerMock = new Mock<ILogger<TicketService>>();
        _service = new TicketService(
            _ticketRepositoryMock.Object, 
            _authRepositoryMock.Object, 
            _unitOfWorkMock.Object, 
            _mapperMock.Object, 
            _LoggerMock.Object);
    }

    [Fact]
    public async Task GetTicketByIdAsync_ReturnsNotFound_WhenTicketDoesNotExist()
    {
        _ticketRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Ticket?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid(), UserTestData.CreateAdminUser());

        result.Status.Should().Be(ServiceStatus.NotFound);
        result.ErrorMessage.Should().Be("Ticket not found");
    }
}
