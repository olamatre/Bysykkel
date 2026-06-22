using Api.Concepts.Stativ.Queries;
using CQRS;
using Microsoft.AspNetCore.Mvc;

namespace Api.Concepts.Stativ;

[ApiController]
[Route("stativ")]
public sealed class StativController(IQueryDispatcher queryDispatcher) : ControllerBase
{
    // TODO: Bruk kartkoordinater for å begrense antall stativer som returneres, basert på en radius fra et punkt
    [HttpGet]
    public Task<IReadOnlyList<StativViewModel>> HentAlleStativer(CancellationToken cancellationToken) =>
        queryDispatcher.Dispatch<GetStativQuery, IReadOnlyList<StativViewModel>>(new GetStativQuery(), cancellationToken);
}
