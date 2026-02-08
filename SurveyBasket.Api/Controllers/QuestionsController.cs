using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Api.Contract.Common;
using SurveyBasket.Api.Contract.Question;

namespace SurveyBasket.Api.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]

public class QuestionsController(IQuestionServices questionServices) : ControllerBase
{
    private readonly IQuestionServices _questionServices = questionServices;

    [HttpGet("")]
    [HasPermission(Permission.GetQuestions)]

    public async Task<IActionResult> GetAll ([FromRoute] int pollId ,[FromQuery]RequestFilter filter , CancellationToken cancellationToken )
    {
        var resualt = await _questionServices.GetAllAsync(pollId, filter, cancellationToken);
        
        return resualt.IsSuccess
            ? Ok(resualt.Value)
            : resualt.ToProblem();
    }
    [HttpGet("{id}")]
    [HasPermission(Permission.GetQuestions)]

    public async Task<IActionResult> Get ([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var resualt = await _questionServices.GetAsync(pollId, id , cancellationToken);
        return resualt.IsSuccess
           ? Ok(resualt.Value)
           : resualt.ToProblem();
    }

    [HttpPost("")]
    [HasPermission(Permission.AddQuestions)]

    public async Task<IActionResult> Create([FromRoute] int pollId , [FromBody] RequestQuestion request ,CancellationToken cancellationToken)
    {
       var resualt = await _questionServices.CreateAsync(pollId, request , cancellationToken);
        return resualt.IsSuccess
            ?  CreatedAtAction(nameof(Get), new { pollId = pollId, id = resualt.Value.Id }, resualt.Value)
            : resualt.ToProblem();
    }

    [HttpPut("{id}")]
    [HasPermission(Permission.UpdatQuestions)]


    public async Task<IActionResult> Update([FromRoute] int pollId ,[FromRoute] int id , [FromBody] RequestQuestion request ,CancellationToken cancellationToken)
    {
       var resualt = await _questionServices.UpdateAsync(pollId, id , request , cancellationToken);
        return resualt.IsSuccess
            ?  NoContent()
            : resualt.ToProblem();
    }

    [HttpPut("{id}/ToggleStutas")]
    [HasPermission(Permission.UpdatQuestions)]

    public async Task<IActionResult> ToggleStatus([FromRoute] int id,[FromRoute] int pollId , CancellationToken cancellationToken)
    {
        var resault = await _questionServices.ToggleStatusAsync(id ,pollId, cancellationToken);
        return resault.IsFaliure
      ? resault.ToProblem()
      : NoContent();

    }
}
