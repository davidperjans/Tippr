using Tippr.Domain.Entities;
using Tippr.Domain.Enums;

namespace Tippr.Application.DTOs.Tournaments
{
    public record TournamentDto(Guid Id, string Name, int Year); 
}
