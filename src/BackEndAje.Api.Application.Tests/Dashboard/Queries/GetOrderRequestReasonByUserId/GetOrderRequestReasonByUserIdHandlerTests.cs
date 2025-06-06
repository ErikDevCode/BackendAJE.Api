namespace BackEndAje.Api.Application.Tests.Dashboard.Queries.GetOrderRequestReasonByUserId
{
    using BackEndAje.Api.Application.Dashboard.Queries.GetOrderRequestReasonByUserId;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetOrderRequestReasonByUserIdHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IOrderRequestRepository> _orderRequestRepoMock;
        private readonly GetOrderRequestReasonByUserIdHandler _handler;

        public GetOrderRequestReasonByUserIdHandlerTests()
        {
            this._userRepoMock = new Mock<IUserRepository>();
            this._orderRequestRepoMock = new Mock<IOrderRequestRepository>();
            this._handler = new GetOrderRequestReasonByUserIdHandler(this._userRepoMock.Object, this._orderRequestRepoMock.Object);
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
        public async Task Handle_ShouldReturnCountsPerReason_BasedOnRole(string roleName, int? expectedSupervisorId, int? expectedVendedorId)
        {
            // Arrange
            var userId = 10;
            var request = new GetOrderRequestReasonByUserIdQuery(
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
                    new UserRole
                    {
                        Role = new Role { RoleName = roleName },
                    },
                },
            };

            this._userRepoMock.Setup(r => r.GetUserByIdAsync(userId)).ReturnsAsync(user);

            var reasons = new Dictionary<string, int>
            {
                { "Instalación", 1 },
                { "Retiro", 2 },
                { "Cambio de Equipo", 3 },
                { "Servicio Técnico", 4 },
            };

            foreach (var reason in reasons)
            {
                this._orderRequestRepoMock.Setup(r =>
                    r.GetTotalOrderRequestReasonCount(
                        reason.Value,
                        expectedSupervisorId,
                        expectedVendedorId,
                        request.regionId,
                        request.cediId,
                        request.zoneId,
                        request.route,
                        request.month,
                        request.year))
                .ReturnsAsync(reason.Value * 10);
            }

            // Act
            var result = await this._handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(4);
            result.First().Should().BeOfType<GetOrderRequestReasonByUserIdResult>();
            result.Select(r => r.Value).Should().BeEquivalentTo(new[] { 10, 20, 30, 40 });
        }
    }
}