using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Application.PredictionGroups.Models;
using Tippr.Domain.Entities;
using Tippr.Domain.Enums;

namespace Tippr.Application.PredictionGroups.Queries.GetMyPredictionGroups
{
    public class GetMyPredictionGroupsQueryHandler : IRequestHandler<GetMyPredictionGroupsQuery, Result<IReadOnlyCollection<PredictionGroupDto>>>
    {
        private readonly IPredictionGroupRepository _groupRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IRepository<Tournament> _tournamentRepository;
        private readonly IMapper _mapper;

        public GetMyPredictionGroupsQueryHandler(
            IPredictionGroupRepository groupRepository,
            ICurrentUserService currentUser,
            IRepository<Tournament> tournamentRepository,
            IMapper mapper)
        {
            _groupRepository = groupRepository;
            _currentUser = currentUser;
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyCollection<PredictionGroupDto>>> Handle(GetMyPredictionGroupsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                return Result<IReadOnlyCollection<PredictionGroupDto>>.Failure("User is not authenticated.");

            var groups = await _groupRepository.GetForUserAsync(userId, cancellationToken);

            var tournamentIds = groups.Select(g => g.TournamentId).Distinct().ToList();
            var tournaments = await _tournamentRepository.ListAsync(
                t => tournamentIds.Contains(t.Id),
                cancellationToken);

            var tournamentsById = tournaments.ToDictionary(t => t.Id, t => t.Name);

            var dtos = groups
                .Select(g =>
                {
                    var dto = _mapper.Map<PredictionGroupDto>(g);

                    var tournamentName = tournamentsById.TryGetValue(g.TournamentId, out var name)
                    ? name
                    : "Unknown";

                    var isOwner = g.Members.Any(m => m.UserId == userId && m.Role == GroupRole.Admin);

                    return dto with
                    {
                        TournamentName = tournamentName,
                        IsOwner = isOwner
                    };
                })
                .ToList()
                .AsReadOnly();

            return Result<IReadOnlyCollection<PredictionGroupDto>>.Success(dtos);
        }
    }
}
