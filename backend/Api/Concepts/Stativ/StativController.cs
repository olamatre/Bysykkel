using Api.Concepts.Stativ.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Api.Concepts.Stativ;

[ApiController]
[Route("stativ")]
public sealed class StativController(IHentAlleStativerQueryHandler queryHandler) : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<StativViewModel>> HentAlleStativer(CancellationToken cancellationToken) =>
        queryHandler.HandleAsync(new GetStativQuery(), cancellationToken);
}
