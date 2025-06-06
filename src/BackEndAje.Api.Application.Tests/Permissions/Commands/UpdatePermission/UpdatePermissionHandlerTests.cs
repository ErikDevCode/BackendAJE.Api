namespace BackEndAje.Api.Application.Tests.Permissions.Commands.UpdatePermission
{
    using AutoMapper;
    using BackEndAje.Api.Application.Dtos.Permissions;
    using BackEndAje.Api.Application.Permissions.Commands.UpdatePermission;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class UpdatePermissionHandlerTests
    {
        private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdatePermissionHandler _handler;

        public UpdatePermissionHandlerTests()
        {
            this._permissionRepositoryMock = new Mock<IPermissionRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new UpdatePermissionHandler(
                this._permissionRepositoryMock.Object,
                this._mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Update_Permission_When_It_Exists()
        {
            // Arrange
            var permissionId = 10;
            var dto = new UpdatePermissionDto
            {
                PermissionId = permissionId,
                PermissionName = "EditarOrdenes",
            };

            var command = new UpdatePermissionCommand { Permission = dto };

            var existingPermission = new Permission
            {
                PermissionId = permissionId,
                PermissionName = "NombreAnterior",
            };

            this._permissionRepositoryMock
                .Setup(repo => repo.GetAllPermissionsAsync())
                .ReturnsAsync(new List<Permission> { existingPermission });

            // Act
            var result = await this._handler.Handle(command, default);

            // Assert
            result.Should().Be(Unit.Value);
            this._mapperMock.Verify(m => m.Map(dto, existingPermission), Times.Once);
            this._permissionRepositoryMock.Verify(repo => repo.UpdatePermissionAsync(existingPermission), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Permission_Does_Not_Exist()
        {
            // Arrange
            var dto = new UpdatePermissionDto { PermissionId = 99, PermissionName = "Actualizar" };
            var command = new UpdatePermissionCommand { Permission = dto };

            this._permissionRepositoryMock
                .Setup(repo => repo.GetAllPermissionsAsync())
                .ReturnsAsync(new List<Permission>()); // Vacía

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                this._handler.Handle(command, default));

            exception.Message.Should().Be("Permiso con ID '99' no existe.");
            this._permissionRepositoryMock.Verify(repo => repo.UpdatePermissionAsync(It.IsAny<Permission>()), Times.Never);
        }
    }
}