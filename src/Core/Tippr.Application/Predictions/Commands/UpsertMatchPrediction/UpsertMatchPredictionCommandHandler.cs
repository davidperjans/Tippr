using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Application.Predictions.Models;
using Tippr.Domain.Entities;
using Tippr.Domain.Enums;

namespace Tippr.Application.Predictions.Commands.UpsertMatchPrediction
{
    public class UpsertMatchPredictionCommandHandler : IRequestHandler<UpsertMatchPredictionCommand, Result<MatchPredictionDto>>
    {
        private readonly IRepository<MatchPrediction> _predictionRepository;
        private readonly IRepository<PredictionGroup> _groupRepository;
        private readonly IRepository<Match> _matchRepository;
        private readonly IRepository<PredictionGroupMember> _groupMemberRepository;
        private readonly IRepository<PredictionGroupSettings> _groupSettingsRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpsertMatchPredictionCommandHandler(
            IRepository<MatchPrediction> predictionRepostiory,
            IRepository<PredictionGroup> groupRepostiory,
            IRepository<Match> matchRepository,
            IRepository<PredictionGroupMember> groupMemberRepository,
            IRepository<PredictionGroupSettings> groupSettingsRepository,
            ICurrentUserService currentUser,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _predictionRepository = predictionRepostiory;
            _groupRepository = groupRepostiory;
            _matchRepository = matchRepository;
            _groupMemberRepository = groupMemberRepository;
            _groupSettingsRepository = groupSettingsRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<MatchPredictionDto>> Handle(UpsertMatchPredictionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrWhiteSpace(userId))
                return Result<MatchPredictionDto>.Failure("User not authenticated.");

            // Check if match exists
            var match = await _matchRepository.GetByIdAsync(request.MatchId, cancellationToken);
            if (match == null)
                return Result<MatchPredictionDto>.Failure("Match not found.");


            // If groupId exists, check that group exists and belongs to same tournament
            PredictionGroup? group = null;
            if (request.PredictionGroupId.HasValue)
            {
                group = await _groupRepository.GetByIdAsync(request.PredictionGroupId.Value, cancellationToken);

                if (group == null)
                    return Result<MatchPredictionDto>.Failure("Prediction group not found.");

                if (group.TournamentId != match.TournamentId)
                    return Result<MatchPredictionDto>.Failure("Prediction group belongs to a different tournament.");

                var rulesResult = await EnsureUserCanPredictAsync(
                    userId, match, group, cancellationToken);

                if (!rulesResult.IsSuccess)
                    return Result<MatchPredictionDto>.Failure(rulesResult.Errors.ToArray());
            }
            else
            {
                if (match.Status != MatchStatus.NotStarted || DateTime.UtcNow >= match.KickoffUtc)
                    return Result<MatchPredictionDto>.Failure("Prediction deadline passed.");
            }

            var existingList = await _predictionRepository.ListAsync(
                x =>
                    x.MatchId == request.MatchId &&
                    x.UserId == userId &&
                    x.PredictionGroupId == request.PredictionGroupId,
                cancellationToken);

            var existingPrediction = existingList.FirstOrDefault();
            if (existingPrediction == null)
            {
                var newPrediction = new MatchPrediction
                {
                    MatchId = request.MatchId,
                    UserId = userId,
                    PredictionGroupId = request.PredictionGroupId,
                    PredictedHomeScore = request.PredictedHomeScore,
                    PredictedAwayScore = request.PredictedAwayScore,
                    SubmittedAtUtc = DateTime.UtcNow,
                    Status = PredictionResultStatus.Pending
                };

                await _predictionRepository.AddAsync(newPrediction, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<MatchPredictionDto>(newPrediction);

                return Result<MatchPredictionDto>.Success(dto);
            }
            else
            {
                // Update existing prediction
                existingPrediction.PredictedHomeScore = request.PredictedHomeScore;
                existingPrediction.PredictedAwayScore = request.PredictedAwayScore;
                existingPrediction.SubmittedAtUtc = DateTime.UtcNow;

                _predictionRepository.Update(existingPrediction);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var dto = _mapper.Map<MatchPredictionDto>(existingPrediction);

                return Result<MatchPredictionDto>.Success(dto);
            }
        }

        private async Task<Result> EnsureUserCanPredictAsync(
            string userId,
            Match match,
            PredictionGroup group,
            CancellationToken cancellationToken)
        {
            var members = await _groupMemberRepository.ListAsync(
                m => m.PredictionGroupId == group.Id && m.UserId == userId,
                cancellationToken);

            if (!members.Any())
                return Result.Failure("User is not member of group.");

            // get group settings
            var settingsList = await _groupSettingsRepository.ListAsync(
                s => s.PredictionGroupId == group.Id,
                cancellationToken);

            var settings = settingsList.SingleOrDefault();
            if (settings == null)
                return Result.Failure("Prediction group settings not configured.");

            // make sure game hasn't started
            if (match.Status != MatchStatus.NotStarted)
                return Result.Failure("Match already started.");

            var nowUtc = DateTime.UtcNow;

            // calculate deadline based on strategy
            DateTime deadlineUtc;

            switch (settings.DeadlineStrategy)
            {
                case PredictionDeadlineStrategy.FixedMinutesBeforeKickoff:
                    deadlineUtc = match.KickoffUtc
                        .AddMinutes(-settings.DeadlineMinutesBeforeKickoff);
                    break;

                case PredictionDeadlineStrategy.FixedDateTime:
                    if (!settings.GlobalLockTimeUtc.HasValue)
                        return Result.Failure("Global lock time not configured.");

                    deadlineUtc = settings.GlobalLockTimeUtc.Value;
                    break;

                default:
                    // Fallback: allow until kickoff if anything is strange.
                    deadlineUtc = match.KickoffUtc;
                    break;
            }

            if (nowUtc >= deadlineUtc)
                return Result.Failure("Prediction deadline passed.");

            return Result.Success();
        }
    }
}
