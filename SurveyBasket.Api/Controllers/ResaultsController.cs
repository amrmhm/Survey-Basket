using Asp.Versioning;

namespace SurveyBasket.Api.Controllers;
[ApiVersion(1, Deprecated = true)]
[ApiVersion(2)]
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[HasPermission(Permission.GetResaults)]

public class ResaultsController(IResualtServices resualtServices) : ControllerBase
{
    private readonly IResualtServices _resualtServices = resualtServices;

    [HttpGet("row-data")]
    public async Task<IActionResult> PollVote([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var resault = await _resualtServices.GetPollVoteAsync(pollId, cancellationToken);
        return resault.IsSuccess
            ? Ok(resault.Value)
            : resault.ToProblem();
    }
    [HttpGet("vote-per-day")]
    public async Task<IActionResult> VotePerDay([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var resault = await _resualtServices.GetVotePerDayAsync(pollId, cancellationToken);
        return resault.IsSuccess
            ? Ok(resault.Value)
            : resault.ToProblem();
    }
    [HttpGet("vote-per-qurstion")]
    public async Task<IActionResult> VotePerQuestion([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var resault = await _resualtServices.GetVotePerQuestionAsync(pollId, cancellationToken);
        return resault.IsSuccess
            ? Ok(resault.Value)
            : resault.ToProblem();
    }
}
