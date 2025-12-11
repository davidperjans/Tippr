using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Teams.Models;
using Tippr.Domain.Entities;

namespace Tippr.Application.Teams.Mapping
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<Team, TeamDto>();
        }
    }
}
