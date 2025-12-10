using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Tournaments.Models;

namespace Tippr.Application.Tournaments.Queries.GetTournamentDetails
{
    public sealed class GetTournamentDetailsQueryHandler : IRequestHandler<GetTournamentDetailsQuery, Result<TournamentDetailsDto>>
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;

        public GetTournamentDetailsQueryHandler(ITournamentRepository tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }

        public async Task<Result<TournamentDetailsDto>> Handle(GetTournamentDetailsQuery request, CancellationToken cancellationToken)
        {
            var tournament = await _tournamentRepository
                .GetDetailsAsync(request.Id, cancellationToken);

            if (tournament == null)
                return Result<TournamentDetailsDto>.Failure("Tournament not found.");

            var dto = _mapper.Map<TournamentDetailsDto>(tournament);

            return Result<TournamentDetailsDto>.Success(dto);
        }
    }
}
