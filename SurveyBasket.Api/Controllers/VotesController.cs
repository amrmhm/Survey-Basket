using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.Api.Contract.Votes;

namespace SurveyBasket.Api.Controllers;
[ApiVersion(1, Deprecated = true)]
[ApiVersion(2)]
[Route("api/polls/{pollId}/vote")]
[ApiController]
[Authorize(Roles = (DefaultRole.Member))]
[EnableRateLimiting(RateLimit.ConcurrencyLimit)]
public class VotesController(IQuestionServices questionServices, IVoteServices voteServices) : ControllerBase
{
    private readonly IQuestionServices _questionServices = questionServices;
    private readonly IVoteServices _voteServices = voteServices;

    [HttpGet("")]
    //[ResponseCache(Duration = 60)] // Do not Have Use ResponseCache Return Status Code 200 Return Ok() 
    //[OutputCache(PolicyName ="Polls")] // Output Cache 

    public async Task<IActionResult> StartVote([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        //this Extentions Methods to Return UserId from Authorize used http context
        var userId =/* "2dd69da0-8b28-4112-af7b-f463f49c0213"; */
        User.GetUserId();
        var resault = await _questionServices.GetAvalibaleAsync(pollId, userId!, cancellationToken);
        return resault.IsSuccess
            ? Ok(resault.Value)
            : resault.ToProblem();
    }
    [HttpPost("")]

    public async Task<IActionResult> Create([FromRoute] int pollId, [FromBody] RequestVotes request, CancellationToken cancellationToken)
    {
        var resault = await _voteServices.AddAsync(pollId, User.GetUserId()!, request, cancellationToken);
        return resault.IsSuccess
            ? Created()
            : resault.ToProblem();
    }
}
