using MediatR;
using Tippr.Application.Common;
using Tippr.Application.Common.Interfaces.Repos;
using Tippr.Application.Common.Interfaces.Services;
using Tippr.Domain.Enums;

namespace Tippr.Application.PredictionGroups.Commands.LeavePredictionGroup
{
    public class LeavePredictionGroupCommandHandler : IRequestHandler<LeavePredictionGroupCommand, Result>
    {
        private readonly IPredictionGroupRepository _groupRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public LeavePredictionGroupCommandHandler(
            IPredictionGroupRepository groupRepository,
            ICurrentUserService currentUser,
            IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(LeavePredictionGroupCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            if (string.IsNullOrWhiteSpace(userId))
                return Result.Failure("User is not authenticated.");

            var group = await _groupRepository.GetWithMembersAndSettingsAsync(
                request.PredictionGroupId,
                cancellationToken);

            if (group == null)
                return Result.Failure("Prediction group not found.");

            var member = group.Members.FirstOrDefault(m => m.UserId == userId);

            if (member == null)
                return Result.Failure("You are not a member of this group.");

            if (member.Role == GroupRole.Admin)
                return Result.Failure("Admin cannot leave the group. Transfer ownership or delete group.");

            group.Members.Remove(member);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
