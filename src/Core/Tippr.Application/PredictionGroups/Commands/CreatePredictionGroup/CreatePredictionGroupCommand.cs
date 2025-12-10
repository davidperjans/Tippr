using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tippr.Application.Common;
using Tippr.Application.DTOs.Groups;
using Tippr.Application.PredictionGroups.Models;

namespace Tippr.Application.Groups.Commands.CreateGroup
{
    public record CreatePredictionGroupCommand(
        string Name,
        Guid TournamentId
    ) : IRequest<Result<PredictionGroupDetailsDto>>;
}
