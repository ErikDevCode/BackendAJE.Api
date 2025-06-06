namespace BackEndAje.Api.Application.Tests.Dashboard.Queries.GetAssetsFromOrderRequestStatusAttendedByUserId
{
    using BackEndAje.Api.Application.Dashboard.Queries.GetAssetsFromOrderRequestStatusAttendedByUserId;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAssetsFromOrderRequestStatusAttendedByUserIdHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly Mock<IOrderRequestRepository> _orderRequestRepoMock = new();
        private readonly GetAssetsFromOrderRequestStatusAttendedByUserIdHandler _handler;

        public GetAssetsFromOrderRequestStatusAttendedByUserIdHandlerTests()
        {
            this._handler = new GetAssetsFromOrderRequestStatusAttendedByUserIdHandler(
                this._userRepoMock.Object, this._orderRequestRepoMock.Object);
        }

        [Theory]
        [InlineData(RolesConst.Administrador)]
        [InlineData(RolesConst.Jefe)]
        [InlineData(RolesConst.ProveedorLogistico)]
        [InlineData(RolesConst.Trade)]
        public async Task Handle_ShouldReturnCount_ForAdminLikeRoles(string roleName)
        {
            // Arrange
            var userId = 10;
            var expectedCount = 5;

            var user = new User
            {
                UserId = userId,
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        Role = new Role { RoleName = roleName },
                    },
                },
            };

            this._userRepoMock.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(user);

            this._orderRequestRepoMock.Setup(r =>
                r.GetTotalAssetFromOrderRequestStatusAttendedCount(
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int?>(),
                    It.IsAny<int?>())
            ).ReturnsAsync(expectedCount);

            var query = new GetAssetsFromOrderRequestStatusAttendedByUserIdQuery(1, 2, 3, 4, 6, 2025)
            {
                UserId = userId,
            };

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Type.Should().Be("Activos Atendidos");
            result.Value.Should().Be(expectedCount);
        }

        [Fact]
        public async Task Handle_ShouldReturnZero_ForUnknownRole()
        {
            // Arrange
            var userId = 30;
            var user = new User
            {
                UserId = userId,
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        Role = new Role { RoleName = "Desconocido" },
                    },
                },
            };

            this._userRepoMock.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(user);

            var query = new GetAssetsFromOrderRequestStatusAttendedByUserIdQuery(1, 2, 3, 4, 6, 2025)
            {
                UserId = userId,
            };

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Type.Should().Be("Activos Atendidos");
            result.Value.Should().Be(0);
        }
    }
}
