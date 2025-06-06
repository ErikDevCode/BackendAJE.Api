namespace BackEndAje.Api.Application.Tests.OrderRequestDocument.Queries.GetOrderRequestDocumentById
{
    using AutoMapper;
    using BackEndAje.Api.Application.OrderRequestDocument.Queries.GetOrderRequestDocumentById;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetOrderRequestDocumentByIdHandlerTests
    {
        private readonly Mock<IOrderRequestRepository> _orderRequestRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetOrderRequestDocumentByIdHandler _handler;

        public GetOrderRequestDocumentByIdHandlerTests()
        {
            this._orderRequestRepoMock = new Mock<IOrderRequestRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetOrderRequestDocumentByIdHandler(this._orderRequestRepoMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedResults_WhenDocumentsExist()
        {
            // Arrange
            var orderRequestId = 1;
            var query = new GetOrderRequestDocumentByIdQuery(orderRequestId);

            var documents = new List<Domain.Entities.OrderRequestDocument>
            {
                new Domain.Entities.OrderRequestDocument { DocumentName = "doc1.pdf", ContentType = "application/pdf" },
                new Domain.Entities.OrderRequestDocument { DocumentName = "doc2.pdf", ContentType = "application/pdf" },
            };

            var results = documents.Select(d => new GetOrderRequestDocumentByIdResult { FileName = d.DocumentName, ContentType = d.ContentType }).ToList();

            this._orderRequestRepoMock.Setup(r => r.GetOrderRequestDocumentByOrderRequestId(orderRequestId)).ReturnsAsync(documents);
            foreach (var doc in documents)
            {
                this._mapperMock.Setup(m => m.Map<GetOrderRequestDocumentByIdResult>(doc))
                    .Returns(new GetOrderRequestDocumentByIdResult { FileName = doc.DocumentName, ContentType = doc.ContentType });
            }

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(results);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenNoDocumentsExist()
        {
            // Arrange
            var orderRequestId = 999;
            var query = new GetOrderRequestDocumentByIdQuery(orderRequestId);

            this._orderRequestRepoMock.Setup(r => r.GetOrderRequestDocumentByOrderRequestId(orderRequestId))
                .ReturnsAsync([]);

            // Act
            Func<Task> act = async () => await this._handler.Handle(query, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"No se encontraron documentos para el OrderRequest con ID: {orderRequestId}.");
        }
    }
}

