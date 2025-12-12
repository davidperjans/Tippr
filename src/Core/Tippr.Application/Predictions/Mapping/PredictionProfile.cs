using AutoMapper;
using Tippr.Application.Predictions.Models;
using Tippr.Domain.Entities;

namespace Tippr.Application.Predictions.Mapping
{
    public class PredictionProfile : Profile
    {
        public PredictionProfile()
        {
            CreateMap<MatchPrediction, MatchPredictionDto>();
        }
    }
}
