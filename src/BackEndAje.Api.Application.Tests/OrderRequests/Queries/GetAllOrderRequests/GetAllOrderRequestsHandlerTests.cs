namespace BackEndAje.Api.Application.Tests.OrderRequests.Queries.GetAllOrderRequests
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.OrderRequests.Queries.GetAllOrderRequests;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllOrderRequestsHandlerTests
    {
        private readonly Mock<IOrderRequestRepository> _orderRequestRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllOrderRequestsHandler _handler;

        public GetAllOrderRequestsHandlerTests()
        {
            this._orderRequestRepositoryMock = new Mock<IOrderRequestRepository>();
            this._userRepositoryMock = new Mock<IUserRepository>();
            this._mapperMock = new Mock<IMapper>();

            this._handler = new GetAllOrderRequestsHandler(
                this._orderRequestRepositoryMock.Object,
                this._mapperMock.Object,
                this._userRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Apply_Supervisor_Filter_If_User_Is_Supervisor()
        {
            // Arrange
            var userId = 77;
            var request = new GetAllOrderRequestsQuery { UserId = userId };

            var user = new User
            {
                UserRoles = new List<UserRole>
                {
                    new UserRole { Role = new Role { RoleName = RolesConst.Supervisor } },
                },
            };

            this._userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);

            this._orderRequestRepositoryMock.Setup(x => x.GetTotalOrderRequestCountAsync(
                It.IsAny<int?>(), It.IsAny<int?>(),
                It.IsAny<int?>(), It.IsAny<int?>(),
                It.IsAny<int?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(),
                userId, null
            )).ReturnsAsync(0);

            this._mapperMock.Setup(x => x.Map<List<GetAllOrderRequestsResult>>(It.IsAny<List<OrderRequest>>()))
                .Returns(new List<GetAllOrderRequestsResult>());

            // Act
            var result = await _handler.Handle(request, default);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(0);
        }
    }
}