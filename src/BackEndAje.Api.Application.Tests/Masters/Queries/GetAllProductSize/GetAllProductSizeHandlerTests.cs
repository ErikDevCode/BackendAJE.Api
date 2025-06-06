namespace BackEndAje.Api.Application.Tests.Masters.Queries.GetAllProductSize
{
    using AutoMapper;
    using BackEndAje.Api.Application.Masters.Queries.GetAllProductSize;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllProductSizeHandlerTests
    {
        private readonly Mock<IMastersRepository> _mastersRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllProductSizeHandler _handler;

        public GetAllProductSizeHandlerTests()
        {
            this._mastersRepositoryMock = new Mock<IMastersRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAllProductSizeHandler(this._mastersRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedProductSizes()
        {
            // Arrange
            var productSizes = new List<ProductSize>
            {
                new ProductSize { ProductSizeId = 1, ProductSizeDescription = "Pequeño" },
                new ProductSize { ProductSizeId = 2, ProductSizeDescription = "Grande" },
            };

            var expectedResult = new List<GetAllProductSizeResult>
            {
                new GetAllProductSizeResult { ProductSizeId = 1, ProductSizeDescription = "Pequeño" },
                new GetAllProductSizeResult { ProductSizeId = 2, ProductSizeDescription = "Grande" },
            };

            this._mastersRepositoryMock
                .Setup(repo => repo.GetAllProductSize())
                .ReturnsAsync(productSizes);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetAllProductSizeResult>>(productSizes))
                .Returns(expectedResult);

            var query = new GetAllProductSizeQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}