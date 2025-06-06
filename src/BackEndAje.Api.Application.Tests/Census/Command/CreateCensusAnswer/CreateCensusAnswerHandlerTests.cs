namespace BackEndAje.Api.Application.Tests.Census.Command.CreateCensusAnswer
{
    using BackEndAje.Api.Application.Census.Commands.CreateCensusAnswer;
    using BackEndAje.Api.Application.Interfaces;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using FluentAssertions;
    using MediatR;
    using Moq;

    public class CreateCensusAnswerHandlerTests
    {
        private readonly Mock<ICensusRepository> _censusRepoMock = new();
        private readonly Mock<IS3Service> _s3ServiceMock = new();
        private readonly Mock<IClientAssetRepository> _clientAssetRepoMock = new();
        private readonly Mock<IAssetRepository> _assetRepoMock = new();
        private readonly Mock<IClientRepository> _clientRepoMock = new();
        private readonly Mock<IUserRepository> _userRepoMock = new();
        private readonly CreateCensusAnswerHandler _handler;

        public CreateCensusAnswerHandlerTests()
        {
            this._handler = new CreateCensusAnswerHandler(
                this._censusRepoMock.Object,
                this._s3ServiceMock.Object,
                this._clientAssetRepoMock.Object,
                this._assetRepoMock.Object,
                this._clientRepoMock.Object,
                this._userRepoMock.Object);
        }

        [Fact]
        public async Task Should_Create_Form_If_Not_Exists()
        {
            // Arrange
            var clientId = 1;
            var form = new CensusForm { CensusFormId = 99, ClientId = clientId };
            this._censusRepoMock.Setup(r => r.GetCensusFormAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync((CensusForm)null!);
            this._censusRepoMock.Setup(r => r.CreateCensusFormAsync(clientId, It.IsAny<string>()))
                .ReturnsAsync(form);

            var command = new CreateCensusAnswerCommand
            {
                ClientId = clientId,
                CreatedBy = 1,
                Answers = new List<CreateCensusAnswerItem>
                {
                    new () { CensusQuestionsId = 1, Answer = "Sí" },
                },
            };

            // Act
            var result = await this._handler.Handle(command, default);

            // Assert
            this._censusRepoMock.Verify(r => r.CreateCensusFormAsync(clientId, It.IsAny<string>()), Times.Once);
            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Should_Throw_When_No_And_Multiple_Answers()
        {
            // Arrange
            var form = new CensusForm { CensusFormId = 10 };
            this._censusRepoMock.Setup(r => r.GetCensusFormAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(form);

            var command = new CreateCensusAnswerCommand
            {
                ClientId = 1,
                CreatedBy = 1,
                Answers =
                [
                    new CreateCensusAnswerItem { CensusQuestionsId = 1, Answer = "No" },
                    new CreateCensusAnswerItem { CensusQuestionsId = 2, Answer = "Otra" }
                ],
            };

            // Act + Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => this._handler.Handle(command, default));
        }

        [Fact]
        public async Task Should_Update_Logo_When_QuestionId_Is_7()
        {
            // Arrange
            var asset = new Domain.Entities.Asset { AssetId = 5, Logo = "Viejo" };
            var clientAsset = new ClientAssets { ClientAssetId = 1, AssetId = 5 };
            var form = new CensusForm { CensusFormId = 15 };

            this._censusRepoMock.Setup(r => r.GetCensusFormAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(form);
            this._clientAssetRepoMock.Setup(r => r.GetClientAssetByIdAsync(1)).ReturnsAsync(clientAsset);
            this._assetRepoMock.Setup(r => r.GetAssetById(5)).ReturnsAsync(asset);

            var command = new CreateCensusAnswerCommand
            {
                ClientId = 1,
                CreatedBy = 1,
                Answers =
                [
                    new CreateCensusAnswerItem
                    {
                        CensusQuestionsId = 7,
                        Answer = "Nuevo",
                        ClientAssetId = 1,
                    },
                ],
            };

            // Act
            var result = await this._handler.Handle(command, default);

            // Assert
            this._assetRepoMock.Verify(r => r.UpdateAssetAsync(It.Is<Domain.Entities.Asset>(a => a.Logo == "Nuevo")), Times.Once);
            result.Should().Be(Unit.Value);
        }

        [Fact]
        public async Task Should_Skip_If_Answer_Already_Exists()
        {
            // Arrange
            var form = new CensusForm { CensusFormId = 12 };
            var existingAnswer = new CensusAnswer { CensusAnswerId = 99 };

            this._censusRepoMock.Setup(r => r.GetCensusFormAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(form);
            this._censusRepoMock.Setup(r => r.GetCensusAnswerAsync(1, form.CensusFormId, null)).ReturnsAsync(existingAnswer);

            var command = new CreateCensusAnswerCommand
            {
                ClientId = 1,
                CreatedBy = 1,
                Answers =
                [
                    new CreateCensusAnswerItem { CensusQuestionsId = 1, Answer = "Sí" }
                ],
            };

            // Act
            var result = await this._handler.Handle(command, default);

            // Assert
            this._censusRepoMock.Verify(r => r.AddCensusAnswer(It.IsAny<CensusAnswer>()), Times.Never);
            result.Should().Be(Unit.Value);
        }
    }
}

