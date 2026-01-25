using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Contract.Votes;
using System.Security.Claims;

namespace SurveyBasket.Api.Controllers;
[Route("api/polls/{pollId}/vote")]
[ApiController]
[Authorize]
public class VotesController(IQuestionServices questionServices , IVoteServices voteServices) : ControllerBase
{
    private readonly IQuestionServices _questionServices = questionServices;
    private readonly IVoteServices _voteServices = voteServices;

    [HttpGet("")]
    public async Task<IActionResult> StartVote ([FromRoute] int pollId , CancellationToken cancellationToken)
    {
        //this Extentions Methods to Return UserId from Authorize used http context
        var userId = User.GetUserId();
        var resault = await _questionServices.GetAvalibaleAsync(pollId, userId!, cancellationToken);
        return resault.IsSuccess 
            ? Ok(resault.Value)
            : resault.ToProblem();
    }
    [HttpPost("")]

    public async Task<IActionResult> Create ([FromRoute] int pollId, [FromBody] RequestVotes request , CancellationToken cancellationToken)
    {
        var resault = await _voteServices.AddAsync(pollId, User.GetUserId()!,request ,cancellationToken);
        return resault.IsSuccess 
            ? Created() 
            : resault.ToProblem();
    }
}
