namespace BackEndAje.Api.Application.Tests.Census.Command.UpdateCensusAnswer
{
    using System.Text;
    using BackEndAje.Api.Application.Census.Commands.UpdateCensusAnswer;
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Moq;

    public class UpdateCensusAnswerHandlerTests
    {
        private readonly Mock<ICensusRepository> _censusRepositoryMock;
        private readonly Mock<IS3Service> _s3ServiceMock;
        private readonly UpdateCensusAnswerHandler _handler;

        public UpdateCensusAnswerHandlerTests()
        {
            this._censusRepositoryMock = new Mock<ICensusRepository>();
            this._s3ServiceMock = new Mock<IS3Service>();
            this._handler = new UpdateCensusAnswerHandler(this._censusRepositoryMock.Object, this._s3ServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateAnswer_WhenValidRequestWithoutImage()
        {
            // Arrange
            var censusAnswerId = 1;
            var command = new UpdateCensusAnswerCommand
            {
                censusAnswerId = censusAnswerId,
                answer = "Nuevo valor",
                CreatedBy = 100,
            };

            var censusAnswer = new CensusAnswer
            {
                CensusAnswerId = censusAnswerId,
                CensusQuestionsId = 5,
                Answer = "Antiguo",
                ClientAssetId = 2,
                CensusFormId = 3,
            };

            var censusForm = new CensusForm
            {
                CensusFormId = 3,
                ClientId = 10,
                MonthPeriod = "202406",
            };

            this._censusRepositoryMock.Setup(r => r.GetCensusAnswerById(censusAnswerId))
                .ReturnsAsync(censusAnswer);

            this._censusRepositoryMock.Setup(r => r.GetCensusFormByIdAsync(censusForm.CensusFormId))
                .ReturnsAsync(censusForm);

            this._censusRepositoryMock.Setup(r => r.UpdateCensusAnswer(It.IsAny<CensusAnswer>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            this._censusRepositoryMock.Verify(r => r.UpdateCensusAnswer(It.Is<CensusAnswer>(a => a.Answer == command.answer)), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUploadImage_WhenImageFileProvided()
        {
            // Arrange
            var censusAnswerId = 1;
            var imageBytes = Encoding.UTF8.GetBytes("imagen");
            var stream = new MemoryStream(imageBytes);
            var formFile = new FormFile(stream, 0, imageBytes.Length, "file", "image.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
            };

            var command = new UpdateCensusAnswerCommand
            {
                censusAnswerId = censusAnswerId,
                answer = "image.jpg",
                CreatedBy = 100,
                ImageFile = formFile,
            };

            var censusAnswer = new CensusAnswer
            {
                CensusAnswerId = censusAnswerId,
                CensusQuestionsId = 5,
                Answer = "Antiguo",
                ClientAssetId = 2,
                CensusFormId = 3,
            };

            var censusForm = new CensusForm
            {
                CensusFormId = 3,
                ClientId = 10,
                MonthPeriod = "202406",
            };

            this._censusRepositoryMock.Setup(r => r.GetCensusAnswerById(censusAnswerId))
                .ReturnsAsync(censusAnswer);

            this._censusRepositoryMock.Setup(r => r.GetCensusFormByIdAsync(censusForm.CensusFormId))
                .ReturnsAsync(censusForm);

            this._s3ServiceMock.Setup(s => s.UploadFileAsync(It.IsAny<Stream>(), "census-images", "10", "202406", "image.jpg"))
                .ReturnsAsync("url/imagen/subida.jpg");

            this._censusRepositoryMock.Setup(r => r.UpdateCensusAnswer(It.IsAny<CensusAnswer>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await this._handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            this._s3ServiceMock.Verify(s => s.UploadFileAsync(It.IsAny<Stream>(), "census-images", "10", "202406", "image.jpg"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCensusFormNotFound()
        {
            // Arrange
            var command = new UpdateCensusAnswerCommand
            {
                censusAnswerId = 1,
                answer = "Nuevo valor",
                CreatedBy = 100,
            };

            this._censusRepositoryMock.Setup(r => r.GetCensusAnswerById(command.censusAnswerId))
                .ReturnsAsync(new CensusAnswer { CensusFormId = 999 });

            this._censusRepositoryMock.Setup(r => r.GetCensusFormByIdAsync(999))
                .ReturnsAsync((CensusForm)null!);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => this._handler.Handle(command, CancellationToken.None));
        }
    }
}

