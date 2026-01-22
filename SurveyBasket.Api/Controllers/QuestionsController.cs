using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Contract.Question;

namespace SurveyBasket.Api.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(IQuestionServices questionServices) : ControllerBase
{
    private readonly IQuestionServices _questionServices = questionServices;

    [HttpGet("")]
    public async Task<IActionResult> GetAll ([FromRoute] int pollId , CancellationToken cancellationToken )
    {
        var resualt = await _questionServices.GetAllAsync(pollId, cancellationToken);
        
        return resualt.IsSuccess
            ? Ok(resualt.Value)
            : resualt.ToProblem();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get ([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var resualt = await _questionServices.GetAsync(pollId, id , cancellationToken);
        return resualt.IsSuccess
           ? Ok(resualt.Value)
           : resualt.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromRoute] int pollId , [FromBody] RequestQuestion request ,CancellationToken cancellationToken)
    {
       var resualt = await _questionServices.CreateAsync(pollId, request , cancellationToken);
        return resualt.IsSuccess
            ?  CreatedAtAction(nameof(Get), new { pollId = pollId, id = resualt.Value.Id }, resualt.Value)
            : resualt.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Create([FromRoute] int pollId ,[FromRoute] int id , [FromBody] RequestQuestion request ,CancellationToken cancellationToken)
    {
       var resualt = await _questionServices.UpdateAsync(pollId, id , request , cancellationToken);
        return resualt.IsSuccess
            ?  NoContent()
            : resualt.ToProblem();
    }

    [HttpPut("{id}/ToggleStutas")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id,[FromRoute] int pollId , CancellationToken cancellationToken)
    {
        var resault = await _questionServices.ToggleStatusAsync(id ,pollId, cancellationToken);
        return resault.IsFaliure
      ? resault.ToProblem()
      : NoContent();

    }
}
