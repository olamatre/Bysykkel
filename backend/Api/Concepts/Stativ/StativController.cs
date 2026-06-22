using Api.Concepts.Stativ.Queries;
using CQRS;
using Microsoft.AspNetCore.Mvc;

namespace Api.Concepts.Stativ;

[ApiController]
[Route("stativ")]
public sealed class StativController(IQueryDispatcher queryDispatcher) : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<StativViewModel>> HentAlleStativer(CancellationToken cancellationToken) =>
        queryDispatcher.Dispatch<GetStativQuery, IReadOnlyList<StativViewModel>>(new GetStativQuery(), cancellationToken);
}
