using Tippr.Domain.Entities;
using Tippr.Domain.Enums;

namespace Tippr.Application.DTOs.Tournaments
{
    public class TournamentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TournamentStatus Status { get; set; }
        public string? HostCountry { get; set; }
    }
}
