using AutoMapper;
using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Domain.Entities;
using Tippr.Domain.Enums;

namespace Tippr.Application.PredictionGroups.Commands.JoinPredictionGroup
{
    public class JoinPredictionGroupByCodeCommandHandler : IRequestHandler<JoinPredictionGroupByCodeCommand, Result>
    {
        private readonly IPredictionGroupRepository _groupRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public JoinPredictionGroupByCodeCommandHandler(
            IPredictionGroupRepository groupRepository,
            ICurrentUserService currentUser,
            IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(JoinPredictionGroupByCodeCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                return Result.Failure("User is not authenticated.");

            var group = await _groupRepository.GetByJoinCodeAsync(
                request.JoinCode.Trim().ToUpperInvariant(),
                cancellationToken);

            if (group == null)
                return Result.Failure("Prediction group not found.");

            if (group.Members.Any(m => m.UserId == userId))
                return Result.Failure("You are already a member of this group.");

            group.Members.Add(new PredictionGroupMember
            {
                UserId = userId,
                Role = GroupRole.Member
            });

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
