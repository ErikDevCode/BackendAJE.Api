namespace BackEndAje.Api.Application.Tests.Notification.Command.SendNotification
{
    using BackEndAje.Api.Application.Hubs;
    using BackEndAje.Api.Application.Notification.Commands.SendNotification;
    using FluentAssertions;
    using MediatR;
    using Microsoft.AspNetCore.SignalR;
    using Moq;

    public class SendNotificationHandlerTests
    {
        private readonly Mock<IHubContext<NotificationHub>> _hubContextMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly Mock<IHubClients> _hubClientsMock;
        private readonly SendNotificationHandler _handler;

        public SendNotificationHandlerTests()
        {
            this._hubContextMock = new Mock<IHubContext<NotificationHub>>();
            this._hubClientsMock = new Mock<IHubClients>();
            this._clientProxyMock = new Mock<IClientProxy>();

            this._hubContextMock.Setup(h => h.Clients).Returns(this._hubClientsMock.Object);
            this._hubClientsMock
                .Setup(c => c.User(It.IsAny<string>()))
                .Returns(this._clientProxyMock.Object);

            this._handler = new SendNotificationHandler(this._hubContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldSendNotificationToUser()
        {
            // Arrange
            var command = new SendNotificationCommand
            {
                User = "123",
                Message = "Nueva notificación",
            };

            this._clientProxyMock
                .Setup(c => c.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(args => args[0].ToString() == "Sistema" && args[1].ToString() == command.Message),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            this._hubClientsMock.Verify(c => c.User(command.User), Times.Once);
            this._clientProxyMock.Verify(
                c => c.SendCoreAsync(
                "ReceiveMessage",
                It.IsAny<object[]>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

