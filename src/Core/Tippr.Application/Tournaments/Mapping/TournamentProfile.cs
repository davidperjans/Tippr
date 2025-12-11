using AutoMapper;
using Tippr.Application.Tournaments.Models;
using Tippr.Domain.Entities;

namespace Tippr.Application.Tournaments.Mapping
{
    public class TournamentProfile : Profile
    {
        public TournamentProfile()
        {
            CreateMap<Tournament, TournamentDto>();

            CreateMap<TournamentGroup, TournamentGroupDto>();

            CreateMap<Tournament, TournamentDetailsDto>()
                .ForMember(d => d.Groups, opt => opt.MapFrom(s => s.Groups))
                .ForMember(d => d.Teams, opt => opt.MapFrom(s => s.Teams))
                .ForMember(d => d.Matches, opt => opt.MapFrom(s => s.Matches));
        }
    }
}
