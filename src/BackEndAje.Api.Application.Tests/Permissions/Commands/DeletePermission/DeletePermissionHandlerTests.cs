namespace BackEndAje.Api.Application.Tests.Permissions.Commands.DeletePermission
{
    using BackEndAje.Api.Application.Dtos.Permissions;
    using BackEndAje.Api.Application.Permissions.Commands.DeletePermission;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class DeletePermissionHandlerTests
    {
        private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
        private readonly DeletePermissionHandler _handler;

        public DeletePermissionHandlerTests()
        {
            this._permissionRepositoryMock = new Mock<IPermissionRepository>();
            this._handler = new DeletePermissionHandler(this._permissionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_Permission_When_It_Exists()
        {
            // Arrange
            var permissionId = 10;
            var command = new DeletePermissionCommand
            {
                DeletePermission = new DeletePermissionDto { PermissionId = permissionId },
            };

            var existingPermission = new Permission { PermissionId = permissionId };

            this._permissionRepositoryMock
                .Setup(repo => repo.GetPermissionByIdAsync(permissionId))
                .ReturnsAsync(existingPermission);

            // Act
            var result = await this._handler.Handle(command, default);

            // Assert
            result.Should().BeTrue();
            this._permissionRepositoryMock.Verify(repo => repo.DeletePermissionAsync(existingPermission), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Permission_Does_Not_Exist()
        {
            // Arrange
            var permissionId = 99;
            var command = new DeletePermissionCommand
            {
                DeletePermission = new DeletePermissionDto { PermissionId = permissionId },
            };

            this._permissionRepositoryMock
                .Setup(repo => repo.GetPermissionByIdAsync(permissionId))
                .ReturnsAsync((Permission?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                this._handler.Handle(command, default));

            exception.Message.Should().Be("Permison con ID '99' no existe.");
            this._permissionRepositoryMock.Verify(repo => repo.DeletePermissionAsync(It.IsAny<Permission>()), Times.Never);
        }
    }
}
