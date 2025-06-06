namespace BackEndAje.Api.Application.Tests.Notification.Command.NotificationMarkAsRead
{
    using BackEndAje.Api.Application.Notification.Commands.NotificationMarkAsRead;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class NotificationMarkAsReadHandlerTests
    {
        private readonly Mock<INotificationRepository> _notificationRepositoryMock;
        private readonly NotificationMarkAsReadHandler _handler;

        public NotificationMarkAsReadHandlerTests()
        {
            this._notificationRepositoryMock = new Mock<INotificationRepository>();
            this._handler = new NotificationMarkAsReadHandler(this._notificationRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldMarkNotificationAsRead()
        {
            // Arrange
            var notificationId = 123;
            var command = new NotificationMarkAsReadCommand
            {
                NotificationId = notificationId,
            };

            this._notificationRepositoryMock
                .Setup(r => r.MarkAsReadAsync(notificationId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            this._notificationRepositoryMock
                .Verify(r => r.MarkAsReadAsync(notificationId), Times.Once);
        }
    }
}

