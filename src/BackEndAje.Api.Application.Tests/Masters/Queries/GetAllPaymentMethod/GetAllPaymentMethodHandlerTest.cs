namespace BackEndAje.Api.Application.Tests.Masters.Queries.GetAllPaymentMethod
{
    using AutoMapper;
    using BackEndAje.Api.Application.Masters.Queries.GetAllPaymentMethod;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllPaymentMethodHandlerTest
    {
        private readonly Mock<IMastersRepository> _mastersRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllPaymentMethodHandler _handler;

        public GetAllPaymentMethodHandlerTest()
        {
            this._mastersRepositoryMock = new Mock<IMastersRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAllPaymentMethodHandler(this._mastersRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedPaymentMethods()
        {
            // Arrange
            var paymentMethods = new List<PaymentMethods>
            {
                new PaymentMethods { PaymentMethodId = 1, PaymentMethod = "Contado" },
                new PaymentMethods { PaymentMethodId = 2, PaymentMethod = "Crédito" },
            };

            var expected = new List<GetAllPaymentMethodResult>
            {
                new GetAllPaymentMethodResult { PaymentMethodId = 1, PaymentMethod = "Contado" },
                new GetAllPaymentMethodResult { PaymentMethodId = 2, PaymentMethod = "Crédito" },
            };

            this._mastersRepositoryMock
                .Setup(repo => repo.GetAllPaymentMethods())
                .ReturnsAsync(paymentMethods);

            this._mapperMock
                .Setup(mapper => mapper.Map<List<GetAllPaymentMethodResult>>(paymentMethods))
                .Returns(expected);

            var query = new GetAllPaymentMethodQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}

