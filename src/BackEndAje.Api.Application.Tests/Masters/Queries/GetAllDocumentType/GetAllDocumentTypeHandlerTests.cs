namespace BackEndAje.Api.Application.Tests.Masters.Queries.GetAllDocumentType
{
    using AutoMapper;
    using BackEndAje.Api.Application.Masters.Queries.GetAllDocumentType;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using Moq;

    public class GetAllDocumentTypeHandlerTests
    {
        private readonly Mock<IMastersRepository> _mastersRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllDocumentTypeHandler _handler;

        public GetAllDocumentTypeHandlerTests()
        {
            this._mastersRepositoryMock = new Mock<IMastersRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._handler = new GetAllDocumentTypeHandler(this._mastersRepositoryMock.Object, this._mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedDocumentTypes()
        {
            // Arrange
            var documentTypes = new List<DocumentType>
            {
                new DocumentType { DocumentTypeId = 1, DocumentTypeName = "DNI" },
                new DocumentType { DocumentTypeId = 2, DocumentTypeName = "RUC" },
            };

            var expected = new List<GetAllDocumentTypeResult>
            {
                new GetAllDocumentTypeResult { DocumentTypeId = 1, DocumentTypeName = "DNI" },
                new GetAllDocumentTypeResult { DocumentTypeId = 2, DocumentTypeName = "RUC" },
            };

            this._mastersRepositoryMock.Setup(r => r.GetAllDocumentType()).ReturnsAsync(documentTypes);
            this._mapperMock.Setup(m => m.Map<List<GetAllDocumentTypeResult>>(documentTypes)).Returns(expected);

            var query = new GetAllDocumentTypeQuery();

            // Act
            var result = await this._handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}

