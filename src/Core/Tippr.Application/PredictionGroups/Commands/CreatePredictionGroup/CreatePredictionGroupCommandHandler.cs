using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Application.DTOs.Groups;
using Tippr.Application.PredictionGroups.Models;
using Tippr.Domain.Entities;
using Tippr.Domain.Enums;

namespace Tippr.Application.Groups.Commands.CreateGroup
{
    public class CreatePredictionGroupCommandHandler
    : IRequestHandler<CreatePredictionGroupCommand, Result<PredictionGroupDetailsDto>>
    {
        private readonly IPredictionGroupRepository _groupRepository;
        private readonly IRepository<Tournament> _tournamentRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePredictionGroupCommandHandler(
            IPredictionGroupRepository groupRepository,
            IRepository<Tournament> tournamentRepository,
            ICurrentUserService currentUser,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _groupRepository = groupRepository;
            _tournamentRepository = tournamentRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PredictionGroupDetailsDto>> Handle(CreatePredictionGroupCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                return Result<PredictionGroupDetailsDto>.Failure("User is not authenticated.");

            var tournament = await _tournamentRepository.GetByIdAsync(request.TournamentId, cancellationToken);

            if (tournament is null)
                return Result<PredictionGroupDetailsDto>.Failure("Tournament not found.");

            var joinCode = GenerateJoinCode();

            var group = new PredictionGroup
            {
                Name = request.Name,
                TournamentId = request.TournamentId,
                JoinCode = joinCode,
                CreatedByUserId = userId,
            };

            // Default settings
            var scoring = new ScoringConfig
            {
                ExactScorePoints = 3,
                OutcomeAndGoalDiffPoints = 2,
                OutcomeOnlyPoints = 1,
                WinnerBonusPoints = 5,
                RunnerUpBonusPoints = 3,
                ThirdPlaceBonusPoints = 2,
                MvpBonusPoints = 5,
                TopScorerBonusPoints = 5
            };

            var settings = new PredictionGroupSettings
            {
                PredictionGroup = group,
                PredictionMode = PredictionMode.BeforeEatchMatch,
                DeadlineStrategy = PredictionDeadlineStrategy.FixedMinutesBeforeKickoff,
                DeadlineMinutesBeforeKickoff = 5,
                ScoringConfig = scoring
            };

            group.Settings = settings;

            // Admin blir första medlem, med rollen Admin
            var ownerMember = new PredictionGroupMember
            {
                PredictionGroup = group,
                UserId = userId,
                Role = GroupRole.Admin
            };

            group.Members.Add(ownerMember);

            await _groupRepository.AddAsync(group, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<PredictionGroupDetailsDto>(group);

            dto = dto with
            {
                TournamentName = tournament.Name,
                Leaderboard = new PredictionGroupLeaderboardDto
                {
                    GroupId = group.Id,
                    GroupName = group.Name,
                    Entries = Array.Empty<LeaderboardEntryDto>()
                }
            };

            return Result<PredictionGroupDetailsDto>.Success(dto);
        }

        private string GenerateJoinCode()
        {
            return Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
        }
    }
}
