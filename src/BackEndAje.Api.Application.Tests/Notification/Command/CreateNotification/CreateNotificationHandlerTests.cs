namespace BackEndAje.Api.Application.Tests.Notification.Command.CreateNotification
{
    using AutoMapper;
    using BackEndAje.Api.Application.Notification.Commands.CreateNotification;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class CreateNotificationHandlerTests
    {
        private readonly Mock<INotificationRepository> _notificationRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateNotificationHandler _handler;

        public CreateNotificationHandlerTests()
        {
            this._notificationRepositoryMock = new Mock<INotificationRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new CreateNotificationHandler(this._notificationRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldMapAndSaveNotificationSuccessfully()
        {
            // Arrange
            var command = new CreateNotificationCommand
            {
                Message = "Mensaje importante",
                UserId = 1,
            };

            var mappedNotification = new Domain.Entities.Notification
            {
                Message = command.Message,
                UserId = command.UserId,
            };

            this._mapperMock
                .Setup(m => m.Map<Domain.Entities.Notification>(command))
                .Returns(mappedNotification);

            this._notificationRepositoryMock
                .Setup(r => r.AddNotificationAsync(mappedNotification))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            this._mapperMock.Verify(m => m.Map<Domain.Entities.Notification>(command), Times.Once);
            this._notificationRepositoryMock.Verify(r => r.AddNotificationAsync(mappedNotification), Times.Once);
        }
    }
}

