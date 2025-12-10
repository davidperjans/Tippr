using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Application.PredictionGroups.Models;
using Tippr.Domain.Entities;

namespace Tippr.Application.PredictionGroups.Queries.GetPredictionGroupDetails
{
    public class GetPredictionGroupDetailsQueryHandler : IRequestHandler<GetPredictionGroupDetailsQuery, Result<PredictionGroupDetailsDto>>
    {
        private readonly IPredictionGroupRepository _groupRepository;
        private readonly IRepository<Tournament> _tournamentRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserReadService _userReadService;
        private readonly IMapper _mapper;

        public GetPredictionGroupDetailsQueryHandler(
            IPredictionGroupRepository groupRepository,
            IRepository<Tournament> tournamentRepository,
            ICurrentUserService currentUser,
            IUserReadService userReadService,
            IMapper mapper)
        {
            _groupRepository = groupRepository;
            _tournamentRepository = tournamentRepository;
            _currentUser = currentUser;
            _userReadService = userReadService;
            _mapper = mapper;
        }

        public async Task<Result<PredictionGroupDetailsDto>> Handle(GetPredictionGroupDetailsQuery request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetWithMembersAndSettingsAsync(request.PredictionGroupId, cancellationToken);

            if (group == null)
                return Result<PredictionGroupDetailsDto>.Failure("Prediction group not found.");

            var tournament = await _tournamentRepository.GetByIdAsync(group.TournamentId, cancellationToken);

            if (tournament == null)
                return Result<PredictionGroupDetailsDto>.Failure("Tournament not found.");

            var dto = _mapper.Map<PredictionGroupDetailsDto>(group);

            var memberUserIds = group.Members
                .Select(m => m.UserId)
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Distinct()
                .ToList();

            var userSummaries = await _userReadService.GetUserSummariesAsync(memberUserIds, cancellationToken);

            var membersWithNames = dto.Members
                .Select(m =>
                {
                    if (userSummaries.TryGetValue(m.UserId, out var user))
                    {
                        var display = user.DisplayName ?? user.UserName;
                        return m with { UserName = display };
                    }

                    return m;
                })
                .ToList()
                .AsReadOnly();

            var leaderboard = new PredictionGroupLeaderboardDto
            {
                GroupId = group.Id,
                GroupName = group.Name,
                Entries = Array.Empty<LeaderboardEntryDto>()
            };

            dto = dto with
            {
                TournamentName = tournament.Name,
                Members = membersWithNames,
                Leaderboard = leaderboard
            };

            return Result<PredictionGroupDetailsDto>.Success(dto);
        }
    }
}
