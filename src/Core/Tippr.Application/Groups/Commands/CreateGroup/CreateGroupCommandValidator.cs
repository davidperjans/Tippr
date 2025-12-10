using FluentValidation;

namespace Tippr.Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
    {
        public CreateGroupCommandValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Group name is required")
            .Length(3, 100).WithMessage("Group name must be between 3 and 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.TournamentId)
                .GreaterThan(0).WithMessage("A valid tournament must be selected");

            RuleFor(x => x.MaxMembers)
                .GreaterThan(1).When(x => x.MaxMembers.HasValue)
                .WithMessage("Max members must be at least 2");
        }
    }
}
