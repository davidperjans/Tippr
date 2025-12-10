using AutoMapper;
using Moq;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Application.PredictionGroups.Mapping;
using Tippr.Application.PredictionGroups.Queries.GetPredictionGroupDetails;
using Tippr.Application.Users.Models;
using Tippr.Domain.Entities;
using Tippr.Domain.Enums;

namespace Tippr.Application.Tests.PredictionGroups
{
    public class GetPredictionGroupDetailsQueryHandlerTests
    {
        private readonly Mock<IPredictionGroupRepository> _groupRepoMock = new();
        private readonly Mock<IRepository<Tournament>> _tournamentRepoMock = new();
        private readonly Mock<ICurrentUserService> _currentUserMock = new();
        private readonly Mock<IUserReadService> _userReadServiceMock = new();
        private readonly IMapper _mapper;

        public GetPredictionGroupDetailsQueryHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PredictionGroupProfile());
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_Should_Map_UserNames_From_UserReadService()
        {
            // Arrange
            var groupId = Guid.NewGuid();
            var tournamentId = Guid.NewGuid();

            var group = new PredictionGroup
            {
                Id = groupId,
                Name = "VM 2026 Kompisgänget",
                TournamentId = tournamentId,
                JoinCode = "ABC12345",
                CreatedByUserId = "owner-1",
                Settings = new PredictionGroupSettings
                {
                    PredictionMode = PredictionMode.BeforeEatchMatch,
                    DeadlineStrategy = PredictionDeadlineStrategy.FixedMinutesBeforeKickoff,
                    DeadlineMinutesBeforeKickoff = 5,
                    ScoringConfig = new ScoringConfig
                    {
                        ExactScorePoints = 3,
                        OutcomeAndGoalDiffPoints = 2,
                        OutcomeOnlyPoints = 1,
                        WinnerBonusPoints = 5,
                        RunnerUpBonusPoints = 3,
                        ThirdPlaceBonusPoints = 2,
                        MvpBonusPoints = 5,
                        TopScorerBonusPoints = 5
                    }
                },
                Members =
            {
                new PredictionGroupMember
                {
                    Id = Guid.NewGuid(),
                    UserId = "user-1",
                    Role = GroupRole.Admin
                },
                new PredictionGroupMember
                {
                    Id = Guid.NewGuid(),
                    UserId = "user-2",
                    Role = GroupRole.Member
                }
            }
            };

            var tournament = new Tournament
            {
                Id = tournamentId,
                Name = "World Cup 2026"
            };

            _groupRepoMock
                .Setup(r => r.GetWithMembersAndSettingsAsync(groupId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(group);

            _tournamentRepoMock
                .Setup(r => r.GetByIdAsync(tournamentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tournament);

            _userReadServiceMock
                .Setup(s => s.GetUserSummariesAsync(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dictionary<string, UserSummaryDto>
                {
                {
                    "user-1",
                    new UserSummaryDto {
                        Id = "user-1",
                        UserName = "david",
                        DisplayName = "David" }
                },
                {
                    "user-2",
                    new UserSummaryDto {
                        Id = "user-2",
                        UserName = "anna",
                        DisplayName = "Anna" }
                }
                });

            var handler = new GetPredictionGroupDetailsQueryHandler(
                _groupRepoMock.Object,
                _tournamentRepoMock.Object,
                _currentUserMock.Object,
                _userReadServiceMock.Object,
                _mapper);

            var query = new GetPredictionGroupDetailsQuery(groupId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            var dto = result.Data!;
            Assert.Equal("World Cup 2026", dto.TournamentName);
            Assert.Equal("VM 2026 Kompisgänget", dto.Name);
            Assert.Equal(2, dto.Members.Count);

            var member1 = dto.Members.First(m => m.UserId == "user-1");
            var member2 = dto.Members.First(m => m.UserId == "user-2");

            Assert.Equal("David", member1.UserName);
            Assert.Equal("Anna", member2.UserName);

            _userReadServiceMock.Verify(s =>
                s.GetUserSummariesAsync(
                    It.Is<IEnumerable<string>>(ids =>
                        ids.Contains("user-1") && ids.Contains("user-2") && ids.Count() == 2),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _groupRepoMock.Verify(r =>
                r.GetWithMembersAndSettingsAsync(groupId, It.IsAny<CancellationToken>()),
                Times.Once);

            _tournamentRepoMock.Verify(r =>
                r.GetByIdAsync(tournamentId, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_When_Group_Not_Found_Returns_Failure()
        {
            // Arrange
            var groupId = Guid.NewGuid();

            _groupRepoMock
                .Setup(r => r.GetWithMembersAndSettingsAsync(groupId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((PredictionGroup?)null);

            var handler = new GetPredictionGroupDetailsQueryHandler(
                _groupRepoMock.Object,
                _tournamentRepoMock.Object,
                _currentUserMock.Object,
                _userReadServiceMock.Object,
                _mapper);

            var query = new GetPredictionGroupDetailsQuery(groupId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("not found", result.Error, StringComparison.OrdinalIgnoreCase);

            _tournamentRepoMock.Verify(
                r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
                Times.Never);

            _userReadServiceMock.Verify(
                s => s.GetUserSummariesAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
