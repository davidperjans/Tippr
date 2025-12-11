using MediatR;
using Tippr.Application.Common;

namespace Tippr.Application.Matches.Commands.DeleteMatch
{
    public sealed record DeleteMatchCommand(Guid Id) : IRequest<Result>;
}
