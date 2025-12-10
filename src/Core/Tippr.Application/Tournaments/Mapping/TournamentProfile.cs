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

            CreateMap<Team, TeamDto>()
                .ForMember(d => d.TournamentGroupId,
                    opt => opt.MapFrom(s => s.TournamentGroupId))
                .ForMember(d => d.GroupCode,
                    opt => opt.MapFrom(s => s.TournamentGroup != null ? s.TournamentGroup.Code : null));

            CreateMap<Match, MatchDto>()
                .ForMember(d => d.TournamentId,
                    opt => opt.MapFrom(s => s.TournamentId))
                .ForMember(d => d.TournamentGroupId,
                    opt => opt.MapFrom(s => s.TournamentGroupId))
                .ForMember(d => d.GroupCode,
                    opt => opt.MapFrom(s => s.TournamentGroup != null ? s.TournamentGroup.Code : null))
                .ForMember(d => d.HomeTeamId,
                    opt => opt.MapFrom(s => s.HomeTeamId))
                .ForMember(d => d.HomeTeamName,
                    opt => opt.MapFrom(s => s.HomeTeam.Name))
                .ForMember(d => d.HomeTeamFifaCode,
                    opt => opt.MapFrom(s => s.HomeTeam.FifaCode))
                .ForMember(d => d.AwayTeamId,
                    opt => opt.MapFrom(s => s.AwayTeamId))
                .ForMember(d => d.AwayTeamName,
                    opt => opt.MapFrom(s => s.AwayTeam.Name))
                .ForMember(d => d.AwayTeamFifaCode,
                    opt => opt.MapFrom(s => s.AwayTeam.FifaCode));

            CreateMap<Tournament, TournamentDetailsDto>()
                .ForMember(d => d.Groups, opt => opt.MapFrom(s => s.Groups))
                .ForMember(d => d.Teams, opt => opt.MapFrom(s => s.Teams))
                .ForMember(d => d.Matches, opt => opt.MapFrom(s => s.Matches));
        }
    }
}
