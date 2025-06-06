namespace BackEndAje.Api.Application.Tests.Dashboard.Queries.GetOrderRequestStatusByUserId
{
    using BackEndAje.Api.Application.Dashboard.Queries.GetOrderRequestStatusByUserId;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetOrderRequestStatusByUserIdHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IOrderRequestRepository> _orderRequestRepoMock;
        private readonly GetOrderRequestStatusByUserIdHandler _handler;

        public GetOrderRequestStatusByUserIdHandlerTests()
        {
            this._userRepoMock = new Mock<IUserRepository>();
            this._orderRequestRepoMock = new Mock<IOrderRequestRepository>();
            this._handler = new GetOrderRequestStatusByUserIdHandler(this._orderRequestRepoMock.Object, this._userRepoMock.Object);
        }

        public static IEnumerable<object[]> RoleTestData =>
            new List<object[]>
            {
                new object[] { RolesConst.Administrador, null, null },
                new object[] { RolesConst.Supervisor, 10, null },
                new object[] { RolesConst.Vendedor, null, 10 },
            };

        [Theory]
        [MemberData(nameof(RoleTestData))]
        public async Task Handle_ShouldReturnCountsForEachStatus_ByRole(string roleName, int? supervisorId, int? vendedorId)
        {
            // Arrange
            var userId = 10;
            var request = new GetOrderRequestStatusByUserIdQuery(
                regionId: 1,
                cediId: 2,
                zoneId: 3,
                route: 4,
                month: 6,
                year: 2025)
            {
                UserId = userId,
            };

            var user = new User
            {
                UserId = userId,
                UserRoles = new List<UserRole>
                {
                    new UserRole { Role = new Role { RoleName = roleName } },
                },
            };

            this._userRepoMock.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(user);

            var statusIds = new Dictionary<string, int>
            {
                { "Atendido", 5 },
                { "Generado", 1 },
                { "Rechazado", 3 },
                { "Aprobado", 2 },
                { "Programado", 4 },
                { "Falso Flete", 6 },
                { "Anulado", 7 },
            };

            foreach (var (_, statusId) in statusIds)
            {
                this._orderRequestRepoMock
                    .Setup(r => r.GetTotalOrderRequestStatusCount(
                        statusId,
                        It.IsAny<int?>(), // supervisorId
                        It.IsAny<int?>(), // vendedorId
                        It.IsAny<int?>(), // regionId
                        It.IsAny<int?>(), // cediId
                        It.IsAny<int?>(), // zoneId
                        It.IsAny<int?>(), // route
                        It.IsAny<int>(),  // month
                        It.IsAny<int>()   // year
                    ))
                    .ReturnsAsync(statusId * 10);
            }

            // Act
            var result = await this._handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(statusIds.Count);

            var expectedValues = statusIds.Values.Select(id => id * 10).ToList();
            result.Select(r => r.Value).Should().BeEquivalentTo(expectedValues);
        }
    }
}

