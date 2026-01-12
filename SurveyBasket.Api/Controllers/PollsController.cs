




using MapsterMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollsServices pollsServices) : ControllerBase
{
    private readonly IPollsServices _pollsServices = pollsServices;



    [HttpGet("")]

    public IActionResult GetAll()
    {
        var poll = _pollsServices.GetAll();
        var responsePoll = poll.Adapt<IEnumerable<ResponsePoll>>();
        return Ok(responsePoll);
    }
    [HttpGet("{id}")]

    public IActionResult Get([FromRoute] int id)
    {
        var poll = _pollsServices.Get(id);
        if (poll == null)

            return NotFound();


        var responsePoll = poll.Adapt<ResponsePoll>();


        return Ok(responsePoll);

    }

    [HttpPost("")]

    public IActionResult Create([FromBody] RequestPoll request)
    {


        var newPoll = _pollsServices.Create(request.Adapt<Poll>());

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);

    }
    [HttpPut("{id}")]

    public IActionResult Update([FromRoute] int id, [FromBody] RequestPoll request)
    {
        var updatePoll = _pollsServices.Update(id, request.Adapt<Poll>());
        if (!updatePoll)
            return NotFound();
        return NoContent();

    }
    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var deletePoll = _pollsServices.Delete(id);
        if (!deletePoll)
            return NotFound();
        return NoContent();
    }
}
