namespace BackEndAje.Api.Application.Tests.OrderRequests.Queries.ExportOrderRequests
{
    using System.Globalization;
    using BackEndAje.Api.Application.Dtos.Const;
    using BackEndAje.Api.Application.OrderRequests.Queries.ExportOrderRequests;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;
    using OfficeOpenXml;

    public class ExportOrderRequestsHandlerTests
    {
        private readonly Mock<IOrderRequestRepository> _orderRequestRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ExportOrderRequestsHandler _handler;

        public ExportOrderRequestsHandlerTests()
        {
            this._orderRequestRepositoryMock = new Mock<IOrderRequestRepository>();
            this._userRepositoryMock = new Mock<IUserRepository>();

            this._handler = new ExportOrderRequestsHandler(
                this._orderRequestRepositoryMock.Object,
                this._userRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Generate_Excel_With_OrderRequests()
        {
            // Arrange
            var userId = 1;
            var user = new User
            {
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        Role = new Role { RoleName = RolesConst.Administrador },
                    },
                },
            };

            var orders = new List<OrderRequest>
            {
                new OrderRequest
                {
                    OrderRequestId = 1,
                    Supervisor = new User { PaternalSurName = "Gomez", MaternalSurName = "Diaz", Names = "Luis" },
                    Sucursal = new Cedi { CediName = "Sucursal 1" },
                    ReasonRequest = new ReasonRequest { ReasonDescription = "Instalación" },
                    NegotiatedDate = DateTime.ParseExact("2025-06-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    TimeWindow = new TimeWindow { TimeRange = "08:00 - 10:00" },
                    WithDrawalReasonId = null,
                    Client = new Client { ClientCode = 123, CompanyName = "Ferretería Central" },
                    Observations = "Observación test",
                    Reference = "REF-123",
                    ProductSize = new ProductSize { ProductSizeDescription = "Grande" },
                    OrderStatus = new OrderStatus { StatusName = "Creado" },
                    CreatedAt = DateTime.Parse("2025-06-01 09:00:00"),
                    UpdatedAt = DateTime.Parse("2025-06-01 10:00:00"),
                },
            };

            this._userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);
            this._orderRequestRepositoryMock.Setup(x => x.GetAllAsync(
                It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
                It.IsAny<int?>(), It.IsAny<int?>(),
                It.IsAny<DateTime?>(), It.IsAny<DateTime?>(),
                null, null
            )).ReturnsAsync(orders);

            var query = new ExportOrderRequestsQuery { UserId = userId };

            // Act
            var result = await this._handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.Length.Should().BeGreaterThan(0);

            using var package = new ExcelPackage(new MemoryStream(result));
            var sheet = package.Workbook.Worksheets["Solicitud"];
            sheet.Should().NotBeNull();

            sheet.Cells[2, 1].Text.Should().Be("1");
            sheet.Cells[2, 2].Text.Should().Be("Gomez Diaz Luis");
            sheet.Cells[2, 9].Text.Should().Be("Ferretería Central");
        }

        [Fact]
        public async Task Handle_Should_Apply_Supervisor_Filter_If_User_Is_Supervisor()
        {
            // Arrange
            var userId = 5;
            var user = new User
            {
                UserRoles = new List<UserRole>
                {
                    new UserRole { Role = new Role { RoleName = RolesConst.Supervisor } },
                },
            };

            this._userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);
            this._orderRequestRepositoryMock.Setup(x => x.GetAllAsync(
                It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
                It.IsAny<int?>(), It.IsAny<int?>(),
                It.IsAny<DateTime?>(), It.IsAny<DateTime?>(),
                userId, null
            )).ReturnsAsync(new List<OrderRequest>());

            var query = new ExportOrderRequestsQuery { UserId = userId };

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            this._orderRequestRepositoryMock.Verify(
                x => x.GetAllAsync(
                It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(),
                It.IsAny<int?>(), It.IsAny<int?>(),
                It.IsAny<DateTime?>(), It.IsAny<DateTime?>(),
                userId, null
            ), Times.Once);
        }
    }
}

