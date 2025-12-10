using AutoMapper;
using Moq;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Application.Groups.Commands.CreateGroup;
using Tippr.Domain.Entities;

namespace Tippr.Application.Tests.PredictionGroups
{
    public class CreatePredictionGroupCommandHandlerTests
    {
        [Fact]
        public async Task Handle_When_User_Not_Authenticated_Returns_Failure()
        {
            // Arrange
            var groupRepo = new Mock<IPredictionGroupRepository>();
            var tournamentRepo = new Mock<IRepository<Tournament>>();
            var currentUser = new Mock<ICurrentUserService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var mapper = new Mock<IMapper>();

            currentUser.Setup(c => c.UserId).Returns((string?)null);

            var handler = new CreatePredictionGroupCommandHandler(
                groupRepo.Object,
                tournamentRepo.Object,
                currentUser.Object,
                unitOfWork.Object,
                mapper.Object);

            var command = new CreatePredictionGroupCommand("My Group", Guid.NewGuid());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("not authenticated", result.Error, StringComparison.OrdinalIgnoreCase);

            groupRepo.Verify(
                r => r.AddAsync(It.IsAny<PredictionGroup>(), It.IsAny<CancellationToken>()),
                Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
