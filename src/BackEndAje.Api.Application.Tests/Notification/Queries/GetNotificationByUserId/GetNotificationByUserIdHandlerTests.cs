namespace BackEndAje.Api.Application.Tests.Notification.Queries.GetNotificationByUserId
{
    using AutoMapper;
    using BackEndAje.Api.Application.Notification.Queries.GetNotificationByUserId;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetNotificationByUserIdHandlerTests
    {
        private readonly Mock<INotificationRepository> _notificationRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetNotificationByUserIdHandler _handler;

        public GetNotificationByUserIdHandlerTests()
        {
            this._notificationRepositoryMock = new Mock<INotificationRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetNotificationByUserIdHandler(this._notificationRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedNotifications()
        {
            // Arrange
            var userId = 42;
            var query = new GetNotificationByUserIdQuery(userId);

            var notifications = new List<Domain.Entities.Notification>
            {
                new Domain.Entities.Notification { Id = 1, UserId = userId, Message = "Mensaje 1" },
                new Domain.Entities.Notification { Id = 2, UserId = userId, Message = "Mensaje 2" },
            };

            var mappedResult = new List<GetNotificationByUserIdResult>
            {
                new GetNotificationByUserIdResult { Id = 1, Message = "Mensaje 1" },
                new GetNotificationByUserIdResult { Id = 2, Message = "Mensaje 2" },
            };

            this._notificationRepositoryMock
                .Setup(repo => repo.GetAllNotificationAsync(userId))!
                .ReturnsAsync(notifications);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetNotificationByUserIdResult>>(notifications))
                .Returns(mappedResult);

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(mappedResult);
            this._notificationRepositoryMock.Verify(repo => repo.GetAllNotificationAsync(userId), Times.Once);
            this._mapperMock.Verify(mapper => mapper.Map<List<GetNotificationByUserIdResult>>(notifications), Times.Once);
        }
    }
}

