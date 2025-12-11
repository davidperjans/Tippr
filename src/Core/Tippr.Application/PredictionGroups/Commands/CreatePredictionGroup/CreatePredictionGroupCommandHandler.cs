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

            var joinCode = await GenerateUniqueJoinCodeAsync();

            var group = new PredictionGroup
            {
                Name = request.Name,
                TournamentId = request.TournamentId,
                JoinCode = joinCode,
                CreatedByUserId = userId,
            };

            // Default settings
            var scoringConfig = new ScoringConfig();

            var settings = new PredictionGroupSettings
            {
                PredictionGroup = group,
                PredictionMode = PredictionMode.BeforeEatchMatch,
                DeadlineStrategy = PredictionDeadlineStrategy.FixedMinutesBeforeKickoff,
                DeadlineMinutesBeforeKickoff = 5,
                ScoringConfig = scoringConfig
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

        private async Task<string> GenerateUniqueJoinCodeAsync()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            while (true)
            {
                var code = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                if (await _groupRepository.IsUniqueJoinCodeAsync(code))
                    return code;
            }
        }
    }
}
