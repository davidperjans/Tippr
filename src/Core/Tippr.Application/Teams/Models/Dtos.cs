using Tippr.Domain.Entities;

namespace Tippr.Application.Teams.Models
{
    public record TeamDto
    {
        public Guid Id { get; init; }
        public Guid TournamentId { get; init; }
        public Guid TournamentGroupId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string FifaCode { get; init; } = string.Empty;
        public string FlagUrl { get; init; } = string.Empty;
    }
}
