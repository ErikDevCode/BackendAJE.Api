namespace BackEndAje.Api.Application.Tests.Masters.Queries.GetAllProductTypes
{
    using AutoMapper;
    using BackEndAje.Api.Application.Masters.Queries.GetAllProductTypes;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllProductTypesHandlerTests
    {
        private readonly Mock<IMastersRepository> _mastersRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllProductTypesHandler _handler;

        public GetAllProductTypesHandlerTests()
        {
            this._mastersRepositoryMock = new Mock<IMastersRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAllProductTypesHandler(this._mastersRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedProductTypes()
        {
            // Arrange
            var productTypes = new List<ProductType>
            {
                new ProductType { ProductTypeId = 1, ProductTypeDescription = "Botella" },
                new ProductType { ProductTypeId = 2, ProductTypeDescription = "Envase" },
            };

            var expectedResult = new List<GetAllProductTypesResult>
            {
                new GetAllProductTypesResult { ProductTypeId = 1, ProductTypeDescription = "Botella" },
                new GetAllProductTypesResult { ProductTypeId = 2, ProductTypeDescription = "Envase" },
            };

            this._mastersRepositoryMock
                .Setup(repo => repo.GetAllProductTypes())
                .ReturnsAsync(productTypes);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetAllProductTypesResult>>(productTypes))
                .Returns(expectedResult);

            var query = new GetAllProductTypesQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}

