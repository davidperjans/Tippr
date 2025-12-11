using AutoMapper;
using Tippr.Application.Matches.Models;
using Tippr.Domain.Entities;

namespace Tippr.Application.Matches.Mapping
{
    public class MatchMapping : Profile
    {
        public MatchMapping()
        {
            CreateMap<Match, MatchDto>()
                .ForMember(d => d.HomeTeamName, opt => opt.MapFrom(s => s.HomeTeam.Name))
                .ForMember(d => d.HomeTeamFifaCode, opt => opt.MapFrom(s => s.HomeTeam.FifaCode))
                .ForMember(d => d.AwayTeamName, opt => opt.MapFrom(s => s.AwayTeam.Name))
                .ForMember(d => d.AwayTeamFifaCode, opt => opt.MapFrom(s => s.AwayTeam.FifaCode));
        }
    }
}
