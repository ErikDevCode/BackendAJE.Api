namespace BackEndAje.Api.Application.Tests.Permissions.Commands.CreatePermission
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.Permissions;
    using BackEndAje.Api.Application.Permissions.Commands.CreatePermission;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class CreatePermissionHandlerTests
    {
        private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreatePermissionHandler _handler;

        public CreatePermissionHandlerTests()
        {
            this._permissionRepositoryMock = new Mock<IPermissionRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new CreatePermissionHandler(
                this._permissionRepositoryMock.Object,
                this._mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Create_Permission_When_Not_Exists()
        {
            // Arrange
            var dto = new CreatePermissionDto { PermissionName = "EditarUsuario" };
            var command = new CreatePermissionCommand { Permission = dto };

            this._permissionRepositoryMock
                .Setup(repo => repo.GetAllPermissionsAsync())
                .ReturnsAsync(new List<Permission>());

            var mappedPermission = new Permission { PermissionName = "EditarUsuario" };

            this._mapperMock
                .Setup(mapper => mapper.Map<Permission>(dto))
                .Returns(mappedPermission);

            // Act
            var result = await this._handler.Handle(command, default);

            // Assert
            result.Should().Be(Unit.Value);
            this._permissionRepositoryMock.Verify(repo => repo.AddPermissionAsync(mappedPermission), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Permission_Already_Exists_Ignoring_Case()
        {
            // Arrange
            var dto = new CreatePermissionDto { PermissionName = "VerReportes" };
            var command = new CreatePermissionCommand { Permission = dto };

            var existingPermissions = new List<Permission>
            {
                new Permission { PermissionName = "verreportes" },
            };

            this._permissionRepositoryMock
                .Setup(repo => repo.GetAllPermissionsAsync())
                .ReturnsAsync(existingPermissions);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                this._handler.Handle(command, default));

            exception.Message.Should().Be("Permiso: 'VerReportes' ya existe.");
            this._permissionRepositoryMock.Verify(repo => repo.AddPermissionAsync(It.IsAny<Permission>()), Times.Never);
        }
    }
}