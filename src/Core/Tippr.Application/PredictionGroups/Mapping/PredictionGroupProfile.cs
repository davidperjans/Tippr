using AutoMapper;
using Tippr.Application.PredictionGroups.Models;
using Tippr.Domain.Entities;

namespace Tippr.Application.PredictionGroups.Mapping
{
    public class PredictionGroupProfile : Profile
    {
        public PredictionGroupProfile()
        {
            // ScoringConfig -> ScoringConfigDto
            CreateMap<ScoringConfig, ScoringConfigDto>();

            // PredictionGroupSettings -> PredictionGroupSettingsDto
            CreateMap<PredictionGroupSettings, PredictionGroupSettingsDto>()
                .ForMember(d => d.Scoring,
                    opt => opt.MapFrom(s => s.ScoringConfig));

            // PredictionGroupMember -> PredictionGroupMemberDto
            CreateMap<PredictionGroupMember, PredictionGroupMemberDto>()
                .ForMember(d => d.UserName,
                    opt => opt.Ignore()); // fylls i senare när vi joinar mot ApplicationUser

            // PredictionGroup -> PredictionGroupDto
            CreateMap<PredictionGroup, PredictionGroupDto>()
                .ForMember(d => d.TournamentName,
                    opt => opt.Ignore()) // fylls i från Tournament
                .ForMember(d => d.MemberCount,
                    opt => opt.MapFrom(s => s.Members.Count))
                .ForMember(d => d.IsOwner,
                    opt => opt.Ignore()); // beror på current user

            // PredictionGroup -> PredictionGroupDetailsDto
            CreateMap<PredictionGroup, PredictionGroupDetailsDto>()
                .ForMember(d => d.TournamentName,
                    opt => opt.Ignore()) // fylls i från Tournament
                .ForMember(d => d.Settings,
                    opt => opt.MapFrom(s => s.Settings))
                .ForMember(d => d.Members,
                    opt => opt.MapFrom(s => s.Members))
                .ForMember(d => d.Leaderboard,
                    opt => opt.Ignore()); // kommer byggas från poäng-logik senare
        }
    }
}
