using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Teams.Models;
using Tippr.Domain.Entities;

namespace Tippr.Application.Teams.Queries.GetTeamByTournamentId
{
    public class GetTeamsByTournamentIdQueryHandler : IRequestHandler<GetTeamsByTournamentIdQuery, Result<List<TeamDto>>>
    {
        private readonly IRepository<Team> _teamRepository;
        private readonly IMapper _mapper;
        public GetTeamsByTournamentIdQueryHandler(IRepository<Team> teamRepository, IMapper mapper)
        {
            _teamRepository = teamRepository;
            _mapper = mapper;
        }
        public async Task<Result<List<TeamDto>>> Handle(GetTeamsByTournamentIdQuery request, CancellationToken cancellationToken)
        {
            var teams = await _teamRepository.ListAsync(x => x.TournamentId == request.Id, cancellationToken);

            var dtos = _mapper.Map<List<TeamDto>>(teams);

            return Result<List<TeamDto>>.Success(dtos);
        }
    }
}
