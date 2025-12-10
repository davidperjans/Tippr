using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Tournaments.Models;

namespace Tippr.Application.Tournaments.Queries.GetTournaments
{
    public sealed class GetTournamentsQueryHandler : IRequestHandler<GetTournamentsQuery, Result<IReadOnlyCollection<TournamentDto>>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        public GetTournamentsQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyCollection<TournamentDto>>> Handle(GetTournamentsQuery request, CancellationToken cancellationToken)
        {
            var tournaments = await _tournamentRepository
                .GetAllWithBasicInfoAsync(cancellationToken);

            var dtos = _mapper.Map<IReadOnlyCollection<TournamentDto>>(tournaments);

            return Result<IReadOnlyCollection<TournamentDto>>.Success(dtos);
        }
    }
}
