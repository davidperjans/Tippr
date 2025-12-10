using AutoMapper;
using Moq;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Tournaments.Mapping;
using Tippr.Application.Tournaments.Queries.GetTournaments;
using Tippr.Domain.Entities;

namespace Tippr.Application.Tests.Tournaments
{
    public class GetTournamentsQueryHandlerTests
    {
        private readonly Mock<ITournamentRepository> _tournamentRepoMock = new();
        private readonly IMapper _mapper;

        public GetTournamentsQueryHandlerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TournamentProfile());
            });

            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Handle_Should_Return_All_Tournaments_As_Dtos()
        {
            // Arrange
            var tournaments = new List<Tournament>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "World Cup 2026",
                    Year = 2026,
                    StartDateUtc = new DateTime(2026, 6, 11, 0, 0, 0, DateTimeKind.Utc),
                    EndDateUtc = new DateTime(2026, 7, 19, 0, 0, 0, DateTimeKind.Utc)
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Euro 2028",
                    Year = 2028,
                    StartDateUtc = new DateTime(2028, 6, 10, 0, 0, 0, DateTimeKind.Utc),
                    EndDateUtc = new DateTime(2028, 7, 10, 0, 0, 0, DateTimeKind.Utc)
                }
            };

            _tournamentRepoMock
                .Setup(r => r.GetAllWithBasicInfoAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(tournaments);

            var handler = new GetTournamentsQueryHandler(
                _tournamentRepoMock.Object,
                _mapper);

            var query = new GetTournamentsQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);

            var dtos = result.Data!;
            Assert.Equal(tournaments.Count, dtos.Count);

            var first = dtos.First(d => d.Id == tournaments[0].Id);
            Assert.Equal(tournaments[0].Name, first.Name);
            Assert.Equal(tournaments[0].Year, first.Year);
            Assert.Equal(tournaments[0].StartDateUtc, first.StartDateUtc);
            Assert.Equal(tournaments[0].EndDateUtc, first.EndDateUtc);

            _tournamentRepoMock.Verify(
                r => r.GetAllWithBasicInfoAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_When_No_Tournaments_Exist_Should_Return_Empty_List_Success()
        {
            // Arrange
            _tournamentRepoMock
                .Setup(r => r.GetAllWithBasicInfoAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Tournament>());

            var handler = new GetTournamentsQueryHandler(
                _tournamentRepoMock.Object,
                _mapper);

            var query = new GetTournamentsQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data!);

            _tournamentRepoMock.Verify(
                r => r.GetAllWithBasicInfoAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
