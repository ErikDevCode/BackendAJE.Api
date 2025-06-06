namespace BackEndAje.Api.Application.Tests.Permissions.Queries.GetAllPermissions
{
    using AutoMapper;
    using BackEndAje.Api.Application.Permissions.Queries.GetAllPermissions;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllPermissionsHandlerTests
    {
        private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllPermissionsHandler _handler;

        public GetAllPermissionsHandlerTests()
        {
            this._permissionRepositoryMock = new Mock<IPermissionRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAllPermissionsHandler(
                this._permissionRepositoryMock.Object,
                this._mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Return_Mapped_Permissions()
        {
            // Arrange
            var permissions = new List<Permission>
            {
                new Permission { PermissionId = 1, PermissionName = "VerUsuarios" },
                new Permission { PermissionId = 2, PermissionName = "EditarOrdenes" },
            };

            var mappedResults = new List<GetAllPermissionsResult>
            {
                new GetAllPermissionsResult { PermissionId = 1, PermissionName = "VerUsuarios" },
                new GetAllPermissionsResult { PermissionId = 2, PermissionName = "EditarOrdenes" },
            };

            this._permissionRepositoryMock
                .Setup(repo => repo.GetAllPermissionsAsync())
                .ReturnsAsync(permissions);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetAllPermissionsResult>>(permissions))
                .Returns(mappedResults);

            var query = new GetAllPermissionsQuery();

            // Act
            var result = await this._handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].PermissionName.Should().Be("VerUsuarios");
            result[1].PermissionName.Should().Be("EditarOrdenes");
        }
    }
}
